import {Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@material-ui/core';
import { Delete, Edit, Visibility } from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import NotificationService from '../../../services/notification.service';
import GenerarGuid from '../../solicitudes/components/funciones/guid.funtions';
import { ClaseInmueble, Inmueble, ItemAvance } from '../constants/fondosolidaridadform.model.type';
import ApiCatalogo from '../../../services/api-catalog.service';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import AddressComponent from '../../../components/modals/address/address.component';
import { Address } from '../../../types/address.type';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import UtilService from '../../../utils/util.service';
import { CLASE_INMUEBLE, ESTADO_PROYECTO } from '../constants/fondoSolidaridad.constants';

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

const Inmuebles = (props: Props) => {

    const classes = useStyles(); 
    const apiCatalogo = ApiCatalogo();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {informacionTiposInmuebles,
           informacionClasesInmuebles,
           informacionItemsAvance,
           estadoProyecto,
           setInmuebles, 
           setClasesInmueble, 
           setItemAvances} = props;  
    const [tiposInmuebles,setTiposInmuebles] = useState<any[]>([]);
    const [tipoVivienda, setTipoVivienda] = useState<any[]>([]);
    
    // Tipo Inmueble
    const [estadoBoton, setEstadoBoton] = useState<string>('AGREGAR TIPO INMUEBLE');
    const [anterior, setAnterior] = useState<string>('');
    const [inmuebleModel, setInmuebleModel] = useState<Inmueble[]>([]);
    const [idAnterior, setIdAnterior] = useState<string>('');

    // Clase Inmueble
    const [listaClasesInmueble, setListaClasesInmueble] = useState<any[]>([]);
    const [estadoBotonClase, setEstadoBotonClase] = useState<string>('AGREGAR CLASE INMUEBLE');
    const [anteriorClase, setAnteriorClase] = useState<string>('');
    const [claseInmuebleModel, setClaseInmuebleModel] = useState<ClaseInmueble[]>([]);
    const [idAnteriorClase, setIdAnteriorClase] = useState<string>('');
    const [open, setOpen] = useState(false);

    // Items Avances
    const [estadoBotonItems, setEstadoBotonItems] = useState<string>('AGREGAR ITEM DE AVANCE');
    const [anteriorItem, setAnteriorItem] = useState<string>('');
    const [itemsModel, setItemsModel] = useState<ItemAvance[]>([]);
    const [idAnteriorItem, setIdAnteriorItem] = useState<string>('');

    useEffect(() => {
        obtenerClasesInmuebles();
        obtenerTiposInmuebles();
        if(informacionTiposInmuebles && informacionTiposInmuebles.length > 0 &&
           informacionClasesInmuebles && informacionClasesInmuebles.length > 0 &&
           informacionItemsAvance && informacionItemsAvance.length > 0){
            obtenerListaInmuebles(informacionTiposInmuebles,informacionClasesInmuebles,informacionItemsAvance);
        }
    },[])

    const formularioInmueble = useForm({
        defaultValues: {
            id: 'id',
            tipoInmueble: '',
            valor: '',
            areaPrivada: '',
            areaConstruida: '',
            tipoVivienda: '',
            claseVivienda: '',
            cantidad: '',
            descripcion: '',
        },
    });

    const formularioClasesInmuebles = useForm({
        defaultValues: {
            id: '',
            claseInmueble: '',
            numeroInmueble: '', 
            direccionInmueble: '',
        },
    });

    const formularioItemsAvance = useForm({
        defaultValues: {
            id: '',
            itemAvance: '',
            porcentaje: '',
        },
    });

    const obtenerListaInmuebles = (informacionTiposInmuebles: any[],informacionClasesInmuebles: any[],informacionItemsAvance: any[]) =>{

        const arrayTipoInmuebles =  informacionTiposInmuebles.map(item => {
              if(item){
                  const inmueble: Inmueble = {
                      id: item.codigo,
                      nombreTipoInmueble: item.tipoInmueble,
                      tipoInmueble: item.codigoTipoInmueble,
                      valor: item.valor,
                      areaPrivada: item.areaPrivada,
                      areaConstruida: item.areaConstruida,
                      nombreVivienda: item.tipoVivienda,
                      tipoVivienda: item.codigoTipoVivienda,
                      claseVivienda: item.codigoClaseVivienda,
                      cantidad: item.cantidad,
                      descripcion: item.descripcion
                  }
                  return inmueble;
              }
              return null;
          })
  
        if(arrayTipoInmuebles.length > 0){
            setInmuebleModel(arrayTipoInmuebles as Inmueble[]);
        }

        const arrayClasesInmuebles =  informacionClasesInmuebles.map(item => {
            if(item){
                const clase: ClaseInmueble = {
                    id: item.codigo,
                    claseInmueble: item.claseVivienda,
                    numeroInmueble: item.numeroVivienda,
                    direccionInmueble: item.direccionInmueble
                }
                return clase;
            }
            return null;
        })

      if(arrayClasesInmuebles.length > 0){
          setClaseInmuebleModel(arrayClasesInmuebles as ClaseInmueble[]);
      }

      const arrayItemsAvance =  informacionItemsAvance.map(item => {
        if(item){
            const itemAvance: ItemAvance = {
                id: item.codigo,
                itemAvance: item.itemAvance,
                porcentaje: item.porcentaje,
            }
            return itemAvance;
        }
        return null;
      })

        if(arrayItemsAvance.length > 0){
            setItemsModel(arrayItemsAvance as ItemAvance[]);
        }
        
      }

    const obtenerTiposInmuebles = async () => {
        await apiFondoSolidaridad
            .getConsultarTiposInmuebles()
            .then(async (response: any) => {
                if (response) {
                    if (response.data) {
                        setTiposInmuebles(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener tipos de inmuebles', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerTiposVivienda = async (idTipoInmueble: number) => {
        await apiFondoSolidaridad
            .getConsultarTiposVivienda(idTipoInmueble)
            .then(async (response: any) => {
                if (response) {
                    if (response.data) {
                        setTipoVivienda(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener tipos de vivienda', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerClasesInmuebles = async () => {
        const TIPO_INMUEBLE = 43;
        await apiCatalogo
            .getObtenerValoresCatalogosPorIdCatalogo(TIPO_INMUEBLE)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setListaClasesInmueble(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener clases de inmuebles', { type: toast.TYPE.ERROR });
            });
    };

    const handleOpenAddress = () => setOpen(true);

    const onCloseAddress = (value: Address) => {
        if (value !== null) {
            const result = setFormDataAddres(value);
            formularioClasesInmuebles.setValue('direccionInmueble', result);
            formularioClasesInmuebles.clearErrors('direccionInmueble');
            setOpen(false);
        } else {
            formularioClasesInmuebles.setValue('direccionInmueble', '');
            setOpen(false);
        }
    };

    const agregarTipoInmueble = async (data: any) => {
        const datoEncontrado = inmuebleModel.findIndex(
            item =>
                item.id === data.id &&
                item.id !== anterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroInmueble(data);
        } else {
            toast('El tipo de inmueble ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroInmueble = async (data: any) => {
        const guid = GenerarGuid();
        const inmuebleNuevo: Inmueble = {
            id: guid.newGuid(),
            tipoInmueble: data.tipoInmueble,
            valor: UtilService.cleanCurrencyValue(data.valor),
            areaPrivada: data.areaPrivada,
            areaConstruida: data.areaConstruida,
            tipoVivienda: data.tipoVivienda,
            claseVivienda: data.claseVivienda,
            cantidad: data.cantidad,
            descripcion: String(data.descripcion).toLocaleUpperCase(),
        };
        let inmuebles: Inmueble[] = [];
        if (estadoBoton === 'AGREGAR TIPO INMUEBLE') {

            setInmuebleModel(oldArray => [
                ...oldArray,
                inmuebleNuevo,
            ]);
            inmuebles = [...inmuebleModel];
            inmuebles.push(inmuebleNuevo);
        } else {
            const temp = [...inmuebleModel];
            const posicionActual = temp.findIndex(
                item => item.id === anterior,
            );

            inmuebleNuevo.id = temp[posicionActual].id;

            if (inmuebleNuevo.id !== temp[posicionActual].id) {
                temp[posicionActual].id =
                inmuebleNuevo.id;
            } else {
                inmuebleNuevo.id = idAnterior;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, inmuebleNuevo);
            inmuebles = temp;
            setInmuebleModel(temp);
        }
        setInmuebles(inmuebles);
        setAnterior('');
        resetTipoInmuebleNuevo();
    };

    const seleccionarRegistro = async (item: Inmueble) => {
        setAnterior(item.id);
        setIdAnterior(item.id);
        obtenerTiposVivienda(Number(item.tipoInmueble));
        formularioInmueble.setValue('id', String(item.id));
        formularioInmueble.setValue('tipoInmueble', String(item.tipoInmueble));
        formularioInmueble.setValue('valor', String(item.valor));
        formularioInmueble.setValue('areaPrivada', String(item.areaPrivada));
        formularioInmueble.setValue('areaConstruida', String(item.areaConstruida));
        formularioInmueble.setValue('tipoVivienda', String(item.tipoVivienda));
        formularioInmueble.setValue('claseVivienda', String(item.claseVivienda));
        formularioInmueble.setValue('cantidad', String(item.cantidad));
        formularioInmueble.setValue('descripcion', String(item.descripcion));
        setEstadoBoton('EDITAR TIPO INMUEBLE');
    };

    const eliminarRegistro = async (id: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...inmuebleModel];
            temp.splice(
                temp.findIndex(item => item.id === id),
                1,
            );
            setInmuebleModel(temp);
            setInmuebles(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetTipoInmuebleNuevo();
        }
    };

    const resetTipoInmuebleNuevo = async () => {
        setEstadoBoton('AGREGAR TIPO INMUEBLE');
        formularioInmueble.clearErrors();
        formularioInmueble.setValue('id', '');
        formularioInmueble.setValue('tipoInmueble', '');
        formularioInmueble.setValue('valor', '');
        formularioInmueble.setValue('areaPrivada', '');
        formularioInmueble.setValue('areaConstruida', '');
        formularioInmueble.setValue('tipoVivienda', '');
        formularioInmueble.setValue('claseVivienda', '');
        formularioInmueble.setValue('cantidad', '');
        formularioInmueble.setValue('descripcion', '');
    };


    const agregarClaseInmueble = async (data: any) => {
        const datoEncontrado = claseInmuebleModel.findIndex(
            item =>
                item.id === data.id &&
                item.id !== anteriorClase,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroClaseInmueble(data);
        } else {
            toast('La clase de inmueble ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroClaseInmueble = async (data: any) => {
        const guid = GenerarGuid();
        const claseInmuebleNuevo: ClaseInmueble = {
            id: guid.newGuid(),
            claseInmueble: data.claseInmueble,
            numeroInmueble: data.numeroInmueble,
            direccionInmueble: data.direccionInmueble
        };
        let claseInmuebles: ClaseInmueble[] = [];
        if (estadoBotonClase === 'AGREGAR CLASE INMUEBLE') {

            setClaseInmuebleModel(oldArray => [
                ...oldArray,
                claseInmuebleNuevo,
            ]);
            claseInmuebles = [...claseInmuebleModel];
            claseInmuebles.push(claseInmuebleNuevo);
        } else {
            const temp = [...claseInmuebleModel];
            const posicionActual = temp.findIndex(
                item => item.id === anteriorClase,
            );

            claseInmuebleNuevo.id = temp[posicionActual].id;

            if (claseInmuebleNuevo.id !== temp[posicionActual].id) {
                temp[posicionActual].id =
                claseInmuebleNuevo.id;
            } else {
                claseInmuebleNuevo.id = idAnteriorClase;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, claseInmuebleNuevo);
            claseInmuebles = temp;
            setClaseInmuebleModel(temp);
        }
        setClasesInmueble(claseInmuebles);
        setAnteriorClase('');
        resetClaseInmuebleNuevo();
    };

    const seleccionarRegistroClaseInmueble = async (item: ClaseInmueble) => {
        setAnteriorClase(item.id);
        setIdAnteriorClase(item.id);
        formularioClasesInmuebles.setValue('id', String(item.id));
        formularioClasesInmuebles.setValue('claseInmueble', item.claseInmueble);
        formularioClasesInmuebles.setValue('numeroInmueble', String(item.numeroInmueble));
        formularioClasesInmuebles.setValue('direccionInmueble', item.direccionInmueble);
        setEstadoBotonClase('EDITAR CLASE INMUEBLE');
    };

    const eliminarRegistroClaseImueble = async (id: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...claseInmuebleModel];
            temp.splice(
                temp.findIndex(item => item.id === id),
                1,
            );
            setClaseInmuebleModel(temp);
            setClasesInmueble(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetClaseInmuebleNuevo();
        }
    };

    const resetClaseInmuebleNuevo = async () => {
        setEstadoBotonClase('AGREGAR CLASE INMUEBLE');
        formularioClasesInmuebles.clearErrors();
        formularioClasesInmuebles.setValue('id', '');
        formularioClasesInmuebles.setValue('claseInmueble', '');
        formularioClasesInmuebles.setValue('numeroInmueble', '');
        formularioClasesInmuebles.setValue('direccionInmueble', '');
    };

    const agregarItemAvance = async (data: any) => {
        const datoEncontrado = itemsModel.findIndex(
            item =>
                item.id === data.id &&
                item.id !== anteriorItem,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroItem(data);
        } else {
            toast('El item ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroItem = async (data: any) => {
        const guid = GenerarGuid();
        const itemNuevo: ItemAvance = {
            id: guid.newGuid(),
            itemAvance: String(data.itemAvance.toLocaleUpperCase()),
            porcentaje: data.porcentaje,
        };
        let items: ItemAvance[] = [];
        if (estadoBotonItems === 'AGREGAR ITEM DE AVANCE') {

            setItemsModel(oldArray => [
                ...oldArray,
                itemNuevo,
            ]);
            items = [...itemsModel];
            items.push(itemNuevo);
        } else {
            const temp = [...itemsModel];
            const posicionActual = temp.findIndex(
                item => item.id === anteriorItem,
            );

            itemNuevo.id = temp[posicionActual].id;

            if (itemNuevo.id !== temp[posicionActual].id) {
                temp[posicionActual].id =
                itemNuevo.id;
            } else {
                itemNuevo.id = idAnteriorItem;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, itemNuevo);
            items = temp;
            setItemsModel(temp);
        }
        setItemAvances(items);
        setAnteriorItem('');
        resetItemNuevo();
    };

    const seleccionarRegistroItem = async (item: ItemAvance) => {
        setAnteriorItem(item.id);
        setIdAnteriorItem(item.id);
        formularioItemsAvance.setValue('id', String(item.id));
        formularioItemsAvance.setValue('itemAvance', item.itemAvance);
        formularioItemsAvance.setValue('porcentaje', String(item.porcentaje));
        setEstadoBotonItems('EDITAR ITEM DE AVANCE');
    };

    const eliminarRegistroItem = async (id: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...itemsModel];
            temp.splice(
                temp.findIndex(item => item.id === id),
                1,
            );
            setItemsModel(temp);
            setItemAvances(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
           resetItemNuevo();
        }
    };

    const resetItemNuevo = async () => {
        setEstadoBoton('AGREGAR ITEM DE AVANCE');
        formularioItemsAvance.clearErrors();
        formularioItemsAvance.setValue('id', '');
        formularioItemsAvance.setValue('itemAvance', '');
        formularioItemsAvance.setValue('porcentaje', '');
    };

    const mostrarTipoInmueble = (idTipoInmueble: number) => {
        const nombreTipoInmueble = tiposInmuebles.find((item: any) => idTipoInmueble === item.codigo)
        return nombreTipoInmueble?.descripcion;
    }

    const mostrarTipoVivienda = (idTipoVivienda: number) => {
        const nombreTipoVivienda = tipoVivienda.find((item: any) => idTipoVivienda === item.codigo)
        return nombreTipoVivienda?.descripcion;
    }

    return (
        <>
        <AddressComponent
            onOpen={open}
            onClose={onCloseAddress}
            control={formularioClasesInmuebles.control}
            getValues={formularioClasesInmuebles.watch}
            tipoDireccion
            setValue={formularioClasesInmuebles.setValue}
        />
        <CHPaper title="Información de Tipos de Inmuebles">
             <Grid container>
             <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                             name="tipoInmueble"
                             control={formularioInmueble.control}
                             rules={{
                                 required: 'Este campo es requerido',
                             }}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHSelect
                                     readOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     value={value}
                                     onChange={e => {
                                         obtenerTiposVivienda(Number(e.target.value));
                                         onChange(e);
                                     }}
                                     label= "Tipo Inmueble *"
                                 >
                                    {tiposInmuebles.map(
                                         (option: any) => (
                                            
                                         <MenuItem key={option.codigo} 
                                             value={option.codigo}
                                         >
                                         {option.descripcion}
                                         </MenuItem>
                                         )
                                     )} 
                             </CHSelect>
                         )}
                     /> 
             </Grid>
             <Grid className={classes.column} item xs={12} md={3}>
                     <Controller
                             name="valor"
                             control={formularioInmueble.control}
                             rules={{
                                 required: 'Este campo es requerido.',
                             }}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHTextField
                                     currencyMask
                                     label="Valor *"
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     InputLabelProps={{ shrink: true }}
                                     value={value}
                                     onChange={onChange}
                                     inputProps={{
                                         style: {
                                             textAlign: 'right',
                                         },
                                     }}
                                 />
                             )}
                         />
                     </Grid>
                     <Grid className={classes.column} item xs={12} md={3}>
                     <Controller
                             name="areaPrivada"
                             control={formularioInmueble.control}
                             rules={{
                                 required: 'Este campo es requerido.',
                             }}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHTextField
                                     numberMask
                                     label="Área Privada *"
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     InputLabelProps={{ shrink: true }}
                                     value={value}
                                     onChange={onChange}
                                     inputProps={{
                                         style: {
                                             textAlign: 'right',
                                         },
                                     }}
                                 />
                             )}
                         />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                     <Controller
                             name="areaConstruida"
                             control={formularioInmueble.control}
                             rules={{
                                 required: 'Este campo es requerido.',
                             }}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHTextField
                                     numberMask
                                     label="Área Construida *"
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     InputLabelProps={{ shrink: true }}
                                     value={value}
                                     onChange={onChange}
                                     inputProps={{
                                         style: {
                                             textAlign: 'right',
                                         },
                                     }}
                                 />
                             )}
                         />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                          <Controller
                                 name="tipoVivienda"
                                 control={formularioInmueble.control}
                                 rules={{
                                     required: 'Este campo es requerido.',
                                 }}
                                 render={({
                                     field: { onChange, value },
                                     fieldState: { error },
                                 }) => (
                                     <CHSelect
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     value={value}
                                     onChange={onChange}
                                     label="Tipo Vivienda *"
                                 >
                                  {tipoVivienda.map(
                                         (option: any) => (
                                            
                                         <MenuItem key={option.codigo} 
                                             value={option.codigo}
                                         >
                                         {option.descripcion}
                                         </MenuItem>
                                         )
                                  )} 
                                 </CHSelect>
                             )}
                         /> 
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                         <Controller
                                 name="claseVivienda"
                                 control={formularioInmueble.control}
                                 rules={{
                                     required: 'Este campo es requerido',
                                 }}
                                 render={({
                                     field: { onChange, value },
                                     fieldState: { error },
                                 }) => (
                                     <CHSelect
                                     readOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     value={value}
                                     onChange={onChange}
                                     label="Clase Vivienda *"
                                 >
                                     {listaClasesInmueble.map(
                                         (option: any) => (
                                             <MenuItem
                                                 key={option.idValor}
                                                 value={option.idValor}
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
                             name="cantidad"
                             control={formularioInmueble.control}
                             rules={{
                                 required: 'Este campo es requerido.',
                             }}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHTextField
                                     numberMask
                                     label="Cantidad *"
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     InputLabelProps={{ shrink: true }}
                                     value={value}
                                     onChange={onChange}
                                 />
                             )}
                         />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={12}>
                     <Controller
                             name="descripcion"
                             control={formularioInmueble.control}
                             rules={{
                                 required: 'Este campo es requerido',
                             }}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHTextField
                                     isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     label="Descripción *"
                                     value={value}
                                     InputLabelProps={{ shrink: true }}
                                     inputProps={{
                                         style: {
                                             textTransform: 'uppercase',
                                         },
                                     }}
                                     onChange={e => {
                                         if(e.target.value.length <= 80){
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
               </Grid>
                <Grid container>
                <Grid
                    className={classes.columnButtonAdd}
                    item
                    xs={12}
                    md={12}
                >
                    {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                         <Button
                         className={classes.buttonAddEdit}
                         variant="outlined"
                         onClick={formularioInmueble.handleSubmit(agregarTipoInmueble)}
                     >
                         {estadoBoton}
                     </Button>
                    )}
                    <Button
                        className={classes.buttonAdd}
                        variant="outlined"
                        onClick={resetTipoInmuebleNuevo}
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
                    Lista de Tipos de Inmuebles
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo del Inmueble</TableCell>
                            <TableCell>Valor</TableCell>
                            <TableCell>Área Privada</TableCell>
                            <TableCell>Área Construida</TableCell>
                            <TableCell>Tipo Vivienda</TableCell>
                            <TableCell>Clase de Vivienda</TableCell>
                            <TableCell>Cantidad</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {inmuebleModel.map(
                            (option: Inmueble) => (
                                <TableRow key={option.id}>
                                    <TableCell>{option.nombreTipoInmueble || mostrarTipoInmueble(Number(option.tipoInmueble))}</TableCell>
                                    <TableCell>{UtilService.formatCurrency(option.valor)}</TableCell>
                                    <TableCell>{option.areaPrivada}</TableCell>
                                    <TableCell>{option.areaConstruida}</TableCell>
                                    <TableCell>{option.nombreVivienda || mostrarTipoVivienda(Number(option.tipoVivienda))}</TableCell>
                                    <TableCell>{Number(option.claseVivienda) === CLASE_INMUEBLE.CASA? 'CASA' : 'APARTAMENTO'}</TableCell>
                                    <TableCell>{option.cantidad}</TableCell>
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
                                            {estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && (
                                                <Visibility />
                                            )}
                                             {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                                                <Edit />
                                            )}
                                        </IconButton>
                                        {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                                        <IconButton
                                               aria-label="Eliminar"
                                               color="primary"
                                               size="small"
                                               onClick={() => {
                                                   eliminarRegistro(
                                                       option.id,
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
        <CHPaper title="Información Clases de Inmuebles">
        
            < Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                       <Controller
                            name="claseInmueble"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioClasesInmuebles.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                    label="Clase Inmueble *"
                                    value={value}
                                    onChange={e => {
                                        formularioClasesInmuebles.clearErrors();
                                        if(e.target.value === 'APARTAMENTO'){
                                            formularioClasesInmuebles.register('numeroInmueble', { required: 'Este campo es requerido.' });
                                            formularioClasesInmuebles.register('direccionInmueble', {
                                                required: {
                                                    value: false,
                                                    message: '',
                                                },
                                            });
                                        }else{
                                            formularioClasesInmuebles.register('direccionInmueble', { required: 'Este campo es requerido.' });
                                            formularioClasesInmuebles.register('numeroInmueble', {
                                                required: {
                                                    value: false,
                                                    message: '',
                                                },
                                            });
                                        }
                                        onChange(e);
                                    }}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                >
                                    {listaClasesInmueble.map(
                                        (option: any) => (
                                            <MenuItem
                                                key={option.idValor}
                                                value={option.descripcion}
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
                            name="numeroInmueble"
                            control={formularioClasesInmuebles.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label={formularioClasesInmuebles.getValues('claseInmueble') === 'APARTAMENTO'? "Número Inmueble *" : "Número Inmueble"}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={e => {
                                        if(e.target.value.length <= 20) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
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
                            name="direccionInmueble"
                            control={formularioClasesInmuebles.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label={formularioClasesInmuebles.getValues('claseInmueble') === 'APARTAMENTO'?"Dirección Inmueble" : "Dirección Inmueble *"}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onClick={handleOpenAddress}
                                    onChange={onChange}
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
                    {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                        <Button
                            className={classes.buttonAddEdit}
                            variant="outlined"
                            onClick={formularioClasesInmuebles.handleSubmit(agregarClaseInmueble)}
                        >
                            {estadoBotonClase}
                        </Button>
                    )}
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetClaseInmuebleNuevo}
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
                    Lista de Clases de Inmuebles
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Clase del Inmueble</TableCell>
                            <TableCell>Número de Inmueble</TableCell>
                            <TableCell>Dirección de Inmueble</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {claseInmuebleModel.map(
                            (option: ClaseInmueble) => (
                                <TableRow key={option.id}>
                                    <TableCell>{option.claseInmueble}</TableCell>
                                    <TableCell>{option.numeroInmueble}</TableCell>
                                    <TableCell>{option.direccionInmueble}</TableCell>
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistroClaseInmueble(
                                                    option,
                                                );
                                            }}
                                        >
                                            {estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && (
                                                <Visibility />
                                            )}
                                             {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                                                <Edit />
                                            )}
                                        </IconButton>
                                        {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                                        <IconButton
                                               aria-label="Eliminar"
                                               color="primary"
                                               size="small"
                                               onClick={() => {
                                                   eliminarRegistroClaseImueble(
                                                       option.id,
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
        <CHPaper title="Lista de Avance en Costo">
        < Grid container>
            <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="itemAvance"
                            control={formularioItemsAvance.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Item Avance *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        );
                                    }}
                                />
                            )}
                        />

            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="porcentaje"
                            control={formularioItemsAvance.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    label="Porcentaje *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
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
                     {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                        <Button
                            className={classes.buttonAddEdit}
                            variant="outlined"
                            onClick={formularioItemsAvance.handleSubmit(agregarItemAvance)}
                        >
                            {estadoBotonItems}
                        </Button>
                     )}
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetItemNuevo}
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
                    Lista de Items de Avance
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Item de Avance</TableCell>
                            <TableCell>Porcentaje</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {itemsModel.map(
                            (option: ItemAvance) => (
                                <TableRow key={option.itemAvance}>
                                    <TableCell>{option.itemAvance}</TableCell>
                                    <TableCell>{String(option.porcentaje).includes('%')? option.porcentaje : `${option.porcentaje}%`}</TableCell>
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistroItem(
                                                    option,
                                                );
                                            }}
                                        >
                                        {estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && (
                                            <Visibility />
                                        )}
                                            {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                                            <Edit />
                                        )}
                                        </IconButton>
                                    {estadoProyecto === ESTADO_PROYECTO.EN_CREACION && (
                                        <IconButton
                                               aria-label="Eliminar"
                                               color="primary"
                                               size="small"
                                               onClick={() => {
                                                   eliminarRegistroItem(
                                                       option.id,
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
    </>
    )
}

interface Props {
    estadoProyecto: number;
    informacionTiposInmuebles: any[];
    informacionClasesInmuebles: any[];
    informacionItemsAvance: any[];
    setInmuebles: (value: Inmueble[]) => void;
    setClasesInmueble: (value: ClaseInmueble[]) => void;
    setItemAvances: (value: ItemAvance[]) => void;
}

export default Inmuebles;