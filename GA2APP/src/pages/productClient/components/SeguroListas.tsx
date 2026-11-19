import {Button, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import moment from 'moment';
import React, { useRef, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CloudUploadIcon from '@material-ui/icons/CloudUpload';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import Nullable from './reportes/types/Nullable';
import ApiProducto from '../../../services/api-producto.service';
import NotificationService from '../../../services/notification.service';
import { LoadingComponent } from '../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '0',
            marginTop: '25px',
        },
        buttonProcess: {
            width: '100%',
            marginLeft: '0',
            marginTop: '0',
        }
    }),
);

interface formularioType {
    tipoCredito: undefined |number;
    fecha: null | Date,
    archivo: File | undefined,
}

const SeguroListas = () => {

    const classes = useStyles();
    const apiProducto = ApiProducto();
    const [loading, setLoading] = useState(false);
    const formulario = useForm<formularioType>({
        defaultValues: {
            tipoCredito: undefined,
            fecha: null as Nullable<Date>,
            archivo: undefined,
        },
    });

    const fileInputRef = useRef<HTMLInputElement>(null);

    const handleOpenFileDialog = () => {
      fileInputRef.current?.click();
    };
  
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
      const file = event.target.files?.[0];
      if (!file) return;

      if(file){
        formulario.setValue('archivo',file);
      }
    };

    const onSubmit = async(data: any) => {

    if(!data.archivo){
        NotificationService.genericAlert(
            'Información',
            `Se debe cargar el archivo`,
            'warning'
        );
        return;
    }
        
    const confirmAlert = await NotificationService.confirmAlert(
        `¿Está seguro que desea realizar esta acción ?`,
    );

    if(confirmAlert){
        setLoading(true);

        const formData = new FormData();
        formData.append('file', data.archivo); 
        formData.append('fechaCorte', data.fecha.toISOString());
        formData.append('tipoCredito', data.tipoCredito.toString());

        await apiProducto.postActualizarPlanPagosMasivo(formData)
            .then(async (response: any) => {
                setLoading(false);
                if(response.data){
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                }
            })
            .catch(e => {
                setLoading(false);
                toast(`Error al actualizar plan de pagos masivo ${e.message}`, { type: toast.TYPE.ERROR });
            });
        }
    }

    return(
     <>
        <LoadingComponent loading={loading} />
        <CHPrivateContainer>
            <CHPaper title="Actualización Tasas de Seguros">
                <Grid container>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="tipoCredito"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value }, fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tipo Crédito"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}>
                                    <MenuItem value={1}>LEASING HABITACIONAL</MenuItem>
                                    <MenuItem value={2}>HIPOTECARIO</MenuItem>
                                </CHSelect>
                        )} />
                    </Grid>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="fecha"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha"
                                    inputProps={{ readOnly: true }}
                                    onChange={(date, _) => 
                                        onChange(moment(date).endOf('month').startOf('day').toDate())
                                    }
                                    InputLabelProps={{ shrink: true }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    autoOk
                                    views={['year', 'month']}
                                    format='yyyy-MMMM'
                                    placeholder='YYYY-MM'
                                />
                            )}
                        />
                    </Grid>  
                    <Grid item xs={12} md={4} className={classes.column}>
                            <Button className={classes.button} variant="contained" color="primary"
                                    onClick={handleOpenFileDialog}  startIcon={<CloudUploadIcon />}
                            >
                                Cargar Archivo
                            </Button>
                            <input
                                type="file"
                                ref={fileInputRef}
                                accept=".xls,.xlsx"
                                style={{ display: 'none' }}
                                onChange={handleFileChange}
                            />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Button className={classes.buttonProcess} variant="contained" color="primary"
                                onClick={formulario.handleSubmit(onSubmit)}
                        >
                            PROCESAR
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
     </>
    )
}

export default SeguroListas;