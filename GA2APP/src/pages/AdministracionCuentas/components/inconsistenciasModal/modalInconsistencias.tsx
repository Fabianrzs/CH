import React, { useEffect, useState } from 'react';
import {
  Button,
  createStyles,
  Dialog,
  DialogContent,
  Grid,
  makeStyles,
  Typography,
  CircularProgress,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TableContainer,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Checkbox,
  Paper
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import UtilService from '../../../../utils/util.service';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ConceptoCuenta } from '../types';
import CHDatePickerV5 from '../../../../components/ch-datepicker/CHDatePickerV5';
import CHDatePickerV6 from '../../../../components/ch-datepicker/CHDatepickerV6';
import CHDatePickerV7 from '../../../../components/ch-datepicker/CHDatepickerV7';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import ApiClients from '../../../../services/api-clients.service';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { AppState } from '../../../../store/rootReducer';
import { startGetMovementsAccountsClient, startGetAdministracionCuentasAfiliado } from '../../../../actions/client.action';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import { AdministracionParametroModel } from '../../../../models/requests/create-simulation-client';



const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.paper,
  },
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
  buttonadd: {
    width: '250px',
    height: '50px',
    marginRight: '1rem',
    background: '#43b5f1',
  },
  titleform: {
    width: '100%',
    textAlign: 'center',
    color: '#1C3F6B',
    padding: 1,
    fontWeight: 'bold',
    fontSize: '1rem',
  },
  column: {
    padding: '10px',
    textAlign: 'center'
  },
  select: {

    textAlign: 'left',
    paddingRight: '100px'

  },
  tableCellBlueText: {
    color: 'blue',
  },
  h2: {
    color: '#1C3F6B',
  }
}));

function ModalInconsistencias({
  open,
  onClose,
  catalogState,
  responseAdminCuenta,
  addRow
}: {
  open: any;
  onClose: any;
  catalogState: any;
  responseAdminCuenta: any;
  addRow: any;
}): any {
  const classes = useStyles();
  const api = ApiClients();


  const { handleSubmit, control, getValues, setValue, watch } = useForm({
    defaultValues: {
      mesInconsistencia: Date(),
      añoInconsistencia: Date(),
      tipoDocumentoId: '',
      numeroDocumento: '',
      codigoMilitar: '',
      nombres: '',
      descripcion: '',
    },
  });


  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [dataFormCompleta, setDataFormCompleta] = useState('');
  const [unidadEje, setUnidadEje] = useState('');
  const [loading, setLoading] = useState<boolean>(false);
  const [consultaFinalizada, setConsultaFinalizada] = useState<boolean>(false);

  const today = new Date();
  const identificationType = watch('tipoDocumentoId');

  const searchAdministracionCuentas = async () => {
    try {
      setLoading(true);
      setConsultaFinalizada(false);
      const formData = getValues();
      
      const response = await api.getAdministracionCuentasAfiliado({
        numeroDocumento: formData.numeroDocumento,
        tipoDocumentoId: parseInt(formData.tipoDocumentoId, 10)
      })

      const priNombreAfiliado = response.data.primerNombre || '';
      const segNombreAfiliado = response.data.segundoNombre || '';
      const priApellidoAfiliado = response.data.primerApellido || '';
      const segApellidoAfiliado = response.data.segundoApellido || '';
      const nombresCompletos = `${priNombreAfiliado} ${segNombreAfiliado} ${priApellidoAfiliado} ${segApellidoAfiliado}`.trim();
      const unidadEjecutora = response.data.unidadEjecutora || '';

      setDataFormCompleta(nombresCompletos);
      setUnidadEje(unidadEjecutora);
      setSearchResults(response.data);

      toast('Afiliado encontrado');

      setValue("tipoDocumentoId", "");
      setValue('numeroDocumento', "");
      setConsultaFinalizada(true);
      

    } catch (error) {
      toast.error(`Error al buscar datos: ${error}`);
    }
    setLoading(false);

  };


  return (
    <Dialog
      fullWidth
      maxWidth="sm"
      open={open}
      onClose={onClose}
      aria-labelledby="form-dialog-title"
    >
      <DialogContent>
        {loading}
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <Typography className={classes.h2} variant="h5" component="h6" >
              INCONSISTENCIAS
            </Typography>
          </Grid>
          <Grid item xs={12}>
            <FormControl fullWidth>
              <InputLabel>Calcular Interés Desde</InputLabel>
              <br />
              <br />
              <br />
              <br />
              <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <Grid className={classes.column} item xs={12} md={3}>
                  <Controller
                    name="mesInconsistencia"
                    control={control}
                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                      <CHDatePickerV6
                        error={!!error}
                        helperText={error ? error.message : ""}
                        label="Mes"
                        onChange={onChange}
                        value={value}
                        inputProps={{ readOnly: true }}
                        InputLabelProps={{ shrink: true }}
                        autoOk
                        minDate={new Date("1-01-1950")}
                        maxDate={today}
                      />
                    )}
                  />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                  <Controller
                    name="añoInconsistencia"
                    control={control}
                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                      <CHDatePickerV7
                        error={!!error}
                        helperText={error ? error.message : ""}
                        label="Año"
                        onChange={onChange}
                        value={value}
                        inputProps={{ readOnly: true }}
                        InputLabelProps={{ shrink: true }}
                        autoOk
                        minDate={new Date("1-01-1950")}
                        maxDate={today}
                      />
                    )}
                  />
                </Grid>
              </div>
            </FormControl>
          </Grid>
          <LoadingComponent key="loader" loading={loading} />
          <Grid item xs={12}>
            <Controller
              name="tipoDocumentoId"
              control={control}
              rules={{
                required: 'Este campo es requerido.',
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHSelect
                  error={!!error}
                  label="Tipo Documento"
                  value={value}
                  helperText={error ? error.message : null}
                  onChange={onChange}
                >
                  {catalogState.IdentificationType?.map(
                    (option: IDENTIFICATIONTYPEDTO) => (
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
          <Grid item xs={12}>
            {
              (parseInt(identificationType, 10) === 1 || parseInt(identificationType, 10) === 2) ? (

                <Controller
                  name="numeroDocumento"
                  control={control}
                  rules={{
                    required: 'Este campo es requerido.',
                    minLength: {
                      value: 4,
                      message: 'La longitud mínima del campo es de 4 caracteres',
                    },
                    maxLength: {
                      value: 12,
                      message: 'La longitud máxima del campo es de 12 caracteres',
                    },
                    pattern: /^[0-9]+$/g
                  }}
                  defaultValue=""
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHTextField
                      error={!!error}
                      helperText={error ? "El número de identificación no debe contener letras" : null}
                      label="Número de Documento"
                      value={value}
                      onChange={onChange}
                      type="number"
                      onBlur={searchAdministracionCuentas}
                    />
                  )}
                />
              ) : (
                <Controller
                  name="numeroDocumento"
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
                      label="Número de Documento"
                      value={value}
                      onChange={onChange}
                      onBlur={searchAdministracionCuentas}
                      inputProps={{
                        style: {
                          textTransform: 'uppercase',
                        },
                      }}
                    />
                  )}
                />
              )
            }
          </Grid>

          <Grid item xs={12}>
            <Controller
              name="codigoMilitar"
              control={control}
              rules={{
                pattern: {
                  value: /^[a-zA-Z0-9]*$/,
                  message: 'El código militar puede ser alfanumérico.',
                },
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  label="Código Militar"
                  value={value}
                  onChange={onChange}
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
          <Grid item xs={12}>
            <Controller
              name="nombres"
              rules={{
                pattern: /^[ A-zÀ-ú ]+$/g
              }}
              control={control}
              defaultValue=""
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  label="Nombres"
                  value={value || dataFormCompleta}
                  error={!!error}
                  helperText={error ? 'El nombre solo debe contener letras' : null}
                  fullWidth
                  inputProps={{
                    style: {
                      textTransform: 'uppercase',
                    },
                  }}
                />
              )}
            />
          </Grid>
          <Grid item xs={12}>
            <Controller
              name="descripcion"
              control={control}
              rules={{
                pattern: /^[ A-zÀ-ú ]+$/g
              }}
              defaultValue=""
              render={({ field: { onChange, value } }) => (
                <CHTextField
                  label="Descripción"
                  multiline
                  rows={4}
                  inputProps={{
                    style: {
                      textTransform: 'uppercase',
                    },
                  }}
                />
              )}
            />
          </Grid>
          <Grid container justify="flex-end" spacing={2}>
            {consultaFinalizada && (
              <Grid item>
                <Button variant="outlined" color="primary">
                  Adicionar
                </Button>
              </Grid>
            )}
            <Grid item>
              <Button variant="outlined" color="default" onClick={onClose}>
                Cancelar
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
  );
}

const mapStateToProps = (state: AppState) => ({
  clientState: state.client,
  catalogState: state.catalog,
});
const mapDispatchToProps = {
  startGetMovementsAccountsClient,
};
export default connect(mapStateToProps, mapDispatchToProps)(ModalInconsistencias);
