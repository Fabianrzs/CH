import {
  Button,
  Card,
  Container,
  createStyles,
  Grid,
  makeStyles,
  Theme,
  Typography,
  MenuItem,
} from '@material-ui/core';
import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import dayjs from 'dayjs';
import { startPostArchivoNomina } from '../../../actions/cargue-nomina.action';
import CHSelect from '../../../components/ch-select/CHSelect';
import { EXECUTINGUNITDTO, TYPESFILEDTO } from '../../../types/catalog.type';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import NotificationService from '../../../services/notification.service';
import { NotificationCardModel, NotificationCardModelEnum } from '../../../models/requests/notification-cards-model';
import { AppState } from '../../../store/rootReducer';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    btn: {
      justifyContent: 'center',
      margin: '5px',
      padding: theme.spacing(0),
    },
    column: {
      display: 'flex',
      flexDirection: 'column',
    },
    progressContainer: {
      marginTop: theme.spacing(2),
    },
    progressBar: {
      height: 10,
      borderRadius: 5,
      backgroundColor: '#d0d7df',
      '& .MuiLinearProgress-barColorPrimary': {
        backgroundColor: '#1976d2',
      },
    },
    progressLabel: {
      marginTop: theme.spacing(1),
      textAlign: 'center',
      color: '#1976d2',
      fontWeight: 500,
    },
  })
);

const FileUpload = () => {
  const dispatch = useDispatch();
  const catalogState = useSelector((state: AppState) => state.catalog);
  const classes = useStyles();
  const formularioNomina = useForm();

  const [file, setFile] = useState<File>();
  const [buttonDisable, setButtonDisable] = useState(true);


  const handleFileChosen = (file: any) => {
    if (file !== undefined) {
      setFile(file);
      setButtonDisable(false);
    } else {
      setButtonDisable(true);
    }
  };


  const uploadFile = async (data: any) => {
    
    if (!file) {
      NotificationService.showNotification({
        message: 'Por favor seleccione un archivo.',
        type: NotificationCardModelEnum.warn,
      } as NotificationCardModel);
      return;
    }
  
    const fechaFormateada = dayjs(data.dct_FechaCreacion).format('YYYYMM');
    const formData = new FormData();
    formData.append('file', file);
    formData.append('Uej_Id', data.uej_Id);
    formData.append('Tdc_Id', data.tdc_Id);
    formData.append('Dct_FechaCreacion', fechaFormateada);
    formData.append('Dct_Nombre', file.name);
  
    const filename = file.name;
    const filenameLength = filename.length;
    const nameTipoArchivo = filename.substring(0, 3);
    let nameClasificacionArchivo = filename.substring(3, 6);
    const namePeriodo = filename.substring(6, 12);
    const nameEjecutora = filename.substring(12, 15);
    const formatoArchivoType = file.type;
  
    const formatosDeseados: { [key: string]: string } = {
      'text/plain': 'txt',
      'application/vnd.ms-excel': 'excel',
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': 'excel',
      'application/msword': 'Word',
      'application/vnd.openxmlformats-officedocument.wordprocessingml.document': 'Word',
      'application/pdf': 'PDF',
      'image/jpeg': 'JPEG',
      'image/png': 'PNG',
      'text/csv': 'CSV',
      'application/json': 'JSON',
      'application/xml': 'XML',
      'application/zip': 'ZIP',
      'application/octet-stream': 'Archivo binario',
    };
    const formatosDeseado = formatosDeseados[formatoArchivoType];
  
    const tipoArchivo = catalogState.TypesFile?.find(
      (option: TYPESFILEDTO) => option.id === Number(data.tdc_Id)
    );
    if (tipoArchivo?.tipo === 'APO' && tipoArchivo?.id === 3) {
      nameClasificacionArchivo = 'AOV';
    }
  
    if (
      nameTipoArchivo === tipoArchivo?.tipo &&
      nameClasificacionArchivo === tipoArchivo?.clasificacion &&
      namePeriodo === fechaFormateada &&
      nameEjecutora === data.uej_Id.toString().padStart(3, '0') &&
      filenameLength === 27 &&
      formatoArchivoType === 'text/plain'
    ) {
      // válido
    } else {
      NotificationService.showNotification({
        message: 'El archivo que intenta cargar no coincide con los filtros seleccionados.',
        type: NotificationCardModelEnum.warn,
      } as NotificationCardModel);
      return;
    }
  
    const enviadoga2app = true;
    formData.append('enviadoga2app', String(enviadoga2app));

    await dispatch(
      startPostArchivoNomina(formData ));

    
  };
  
  return (
    <Container>
      <Card style={{ padding: '16px' }}>
        <Typography variant="h6" align="center" className="titulo1">
          Cargar Archivo
        </Typography>
        <Typography variant="h6" align="center" className="titulo2">
          Nómina
        </Typography>

        <form onSubmit={formularioNomina.handleSubmit(uploadFile)}>
          <Grid container spacing={2} style={{ marginTop: 16 }}>
            <Grid item md={4} className={classes.column}>
              <Controller
                name="uej_Id"
                control={formularioNomina.control}
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHSelect
                    error={!!error}
                    label="Unidad Ejecutora"
                    value={value || ''}
                    helperText={error ? error.message : null}
                    onChange={onChange}
                  >
                    {catalogState.ExecutingUnit?.map((option: EXECUTINGUNITDTO) => (
                      <MenuItem key={`menu${option.id}`} value={option.id}>
                        {option.nombre}
                      </MenuItem>
                    ))}
                  </CHSelect>
                )}
              />
            </Grid>

            <Grid item md={4} className={classes.column}>
              <Controller
                name="tdc_Id"
                control={formularioNomina.control}
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHSelect
                    error={!!error}
                    label="Tipo Archivo"
                    value={value || ''}
                    helperText={error ? error.message : null}
                    onChange={onChange}
                  >
                    {catalogState.TypesFile?.map((option: TYPESFILEDTO) => (
                      <MenuItem key={`menu${option.id}`} value={option.id}>
                        {option.descripcion}
                      </MenuItem>
                    ))}
                  </CHSelect>
                )}
              />
            </Grid>

            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="dct_FechaCreacion"
                control={formularioNomina.control}
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHDatePicker
                    InputLabelProps={{ shrink: true }}
                    disableFuture
                    format="yyyy/MM"
                    label="Periodo"
                    onChange={onChange}
                    value={value || ''}
                    views={['year', 'month']}
                    maxDate={new Date()}
                    error={!!error}
                    helperText={error ? error.message : ''}
                  />
                )}
              />
            </Grid>
          </Grid>

          <Grid container spacing={2} className={classes.btn} style={{ marginTop: 16 }}>
            <Grid item xs={6} md={2}>
              <Button variant="contained" component="label" color="primary">
                <input
                  type="file"
                  onChange={e =>
                    handleFileChosen(e.target.files != null ? e.target.files[0] : '')
                  }
                  hidden
                />
                Cargar Archivo
              </Button>
            </Grid>
            <Grid item xs={6} md={2}>
              <Button
                disabled={buttonDisable}
                variant="outlined"
                color="secondary"
                type="submit"
              >
                Enviar Archivo
              </Button>
            </Grid>
          </Grid>
        </form>

        
      </Card>
    </Container>
  );
};

export default FileUpload;
