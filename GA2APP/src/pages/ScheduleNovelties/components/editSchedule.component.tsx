import React, { useEffect, useState } from 'react';
import {
  Grid,
  makeStyles,
  createStyles,
  Button,
  Dialog,
  DialogContent,
  DialogTitle,
  MenuItem,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm, useController } from 'react-hook-form';
import moment from 'moment';
import {
  NovedadesCronograma,
  UnidadEjecutoraCronogramaDTO,
  UnidadEjecutoraCronogramaEditarDTO,
} from '../../../types/cronograma.type';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { ScheduleState } from '../../../reducers/schedule.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHTextFieldV2 from '../../../components/ch-text-field/CHTextFieldV2';
import CHDatePickerV2 from '../../../components/ch-datepicker/CHDatepickerV2';
import CHDatePickerV5 from '../../../components/ch-datepicker/CHDatePickerV5';
import CHSelectV2 from '../../../components/ch-select/CHSelectV2';
import ProductData from '../../productClient/components/ProductData';
import CatalogApi from '../../../services/api-catalog.service';
import ApiSchedule from '../../../services/api-cronograma.service';
import NotificationService from '../../../services/notification.service';
import AuthService from '../../../services/auth.service';


const useStyles = makeStyles(() =>
  createStyles({
    button: {
      width: '120px',
      marginLeft: '.5rem',
      borderRadius: '0%',
      background: '#fec917',
      color: '#1c3b90',
      textAlign: 'right',
      align: 'right',
      marginBottom: '20px',
    },
    column: {
      padding: '10px',
      textAlign: 'center',
    },
    icon: {
      padding: '10px',
      textAlign: 'inherit',
      color: '#1C3F8B',
    },

    icon1: {
      color: '#42b7ef',
    },

    iconRed: {
      color: '#FF0000',
    },
    buttons: {
      color: '#fff',
      width: '120px',
      textAlign: 'center',
      borderRadius: '0%',
      background: '#42b7ef',
      marginBottom: '20px',
      marginLeft: '38rem',
    },

    title: {
      color: '#42b7ef',
      textAlign: 'center',
    },

    table: {
      color: '#f1c3b90ff',
    },
    buttonAddProduct: {
      width: '120px',
      marginLeft: '.5rem',
      borderRadius: '0%',
      background: '#42b7ef',
      color: '#fff',
      textAlign: 'right',
      align: 'right',
      marginBottom: '20px',
    },
  }),
);

const api = CatalogApi();
const apiCronograma = ApiSchedule();

function EditSchedule({
  isOpen,
  data,
  scheduleState,
  StartEditReportDateExecutingUnitsSchedule,
  onClose,
  catalogState,
}: {
  isOpen: boolean;
  data: UnidadEjecutoraCronogramaDTO | undefined;
  scheduleState: ScheduleState;
  StartEditReportDateExecutingUnitsSchedule: (
    unidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditarDTO,
  ) => void;
  onClose: (value: any) => void;
  catalogState: CatalogState;
}) {


  useEffect(() => {
    setHasLoadedFields(false);
    setProducts([]);
  }, [isOpen]);
  const [hasLoadedFields, setHasLoadedFields] = useState(false);
  const [dataProduct, setDataProduct] = useState<NovedadesCronograma[]>(data?.novedades || []);


  const [addProduct, setAddProduct] = useState(false);
  const [products, setProducts] = useState<any[]>([]);


  const classes = useStyles();


  const handleClose = () => {
    const value: any = {};
    setAddProduct(false);
    onClose(value);
  };
  const { handleSubmit, control, setValue, watch, setError, clearErrors } =
    useForm<UnidadEjecutoraCronogramaDTO>({
      defaultValues: {
        descripcionFormato: '',
        descripcionMedioEnvio: '',
        fechaFin: new Date(),
        fechaInicio: new Date(),
        fechaReporte: new Date(),
        fuerzaDescripcion: '',
        id: 0,
        nombreUnidadEjecutora: '',
        periodo: new Date(),
        idFormato: 0,
        idMedioEnvio: 0,
        idFormato2: 0,
        idUnidadFuerza: 0,
        idUnidadEjecutora: 0
      },
    });

  const obtenerConceptos = async () => {
    await api.ObtenerConceptosNovedades().then(async response => {
      if (response) {
        if (response.isSuccess && response.data) {
          setProducts(response.data);
        }
      }
    });
  };



  if (!hasLoadedFields && data != null) {
    obtenerConceptos();
    setHasLoadedFields(true);
    const fields = Object.keys(data);
    fields.forEach(field => {
      const value = data[field as keyof UnidadEjecutoraCronogramaDTO];
      setValue(field as any, value);
    });

    setDataProduct(data.novedades);
  }

  const guardarLog = async (tipoDocumento: any, nDocumento: any, fechaInicio: any, nombreUnidadEjecutora: any, tipoAccion: string, idUnidadEjecutora: number, modulo: string) => {
    const user = AuthService.getUserInfo();
    await apiCronograma.InsertarAudoriaNovedades({
      idModulo: 1,
      modulo: modulo,
      tipoDocumento: tipoDocumento,
      nDocumento: nDocumento,
      detalleModificacion: `Se ha modificado la Unidad Ejecutora ${nombreUnidadEjecutora}`,
      tipoAccion: tipoAccion,
      fechaModificacion: new Date(),
      usuarioModificacion: user.USR_ID,
      fechaInicio: fechaInicio,
      fechaFin: new Date(),
      idUnidadEjecutora: idUnidadEjecutora
    });
  };

  const updateExecutingUnitSchedule = async (data: UnidadEjecutoraCronogramaDTO) => {
    const confirmAlert = await NotificationService.confirmAlert(
      `¿Está seguro de actualizar la información con el período seleccionado ${moment(watch('periodo')).format('MM/YYYY')}?`
    );
    if (confirmAlert) {
      data.novedades = dataProduct;
      StartEditReportDateExecutingUnitsSchedule(data);
      setAddProduct(false);
      onClose(data);

      await guardarLog(1, data.nDocumento, data.fechaInicio, data.nombreUnidadEjecutora, "MODIFICACIÓN", data.idUnidadEjecutora, 'CRONOGRAMA DE NOVEDADES');
    }

  };

  const {
    control: control1,
    watch: watch1,
    reset: reset1,
  } = useForm({
    defaultValues: {
      idNovedad: 0,
      idConceptoNovedad: 0,
      codigoNovedadAlta: '',
      codigoNovedadBaja: '',
      idUnidadEjecutora: 0,
    },
  });

  const addProductToList = () => {
    const product: NovedadesCronograma = {
      idConceptoNovedad: watch1('idConceptoNovedad'),
      codigoNovedadAlta: watch1('codigoNovedadAlta'),
      codigoNovedadBaja: watch1('codigoNovedadBaja'),
      idUnidadEjecutora: watch('idUnidadEjecutora').toString(),
      idNovedad: 0,
      editing: false,
    };

    setDataProduct([...dataProduct, product]);
    setAddProduct(false);
    reset1();
  };

  const {
    control: control2,
    setValue: setValue2,
    getValues: getValues2,
  } = useForm({
    defaultValues: {
      codigoAlta: '',
      codigoBaja: '',
    },
  });


  const validateDate = (data: Date) => {
    const date = moment(data);
    const datePlusAMonth = moment(new Date()).add(2, 'M');
    const dateLessAMonth = moment(new Date()).add(-2, 'M');
    let message = '';
    if (!date.isValid()) {
      message = 'Ingrese una fecha válida';
    } else if (data > datePlusAMonth.toDate()) {
      message = 'Por favor no ingrese una fecha mayor a dos meses';
    } else if (data < dateLessAMonth.toDate()) {
      message = 'Por favor ingrese una fecha menor a dos meses';
    }
    return message !== '' ? message : true;
  };


  const { field } = useController({ name: 'periodo', control });


  useEffect(() => {
    if (!field.value) {
      field.onChange('');
    }
  }, [field]);



  useEffect(() => {
    setValue(
      'periodo',
      new Date(moment(watch('fechaFin')).add(1, 'M').toDate()),
    );


    if (watch('idUnidadEjecutora') === 2) {
      setValue(
        'periodo',
        new Date(moment(watch('fechaFin')).toDate()),
      );
    }
    else {
      setValue(
        'periodo',
        new Date(moment(watch('fechaFin')).add(1, 'M').toDate()),
      );
    }

  }, [watch('fechaFin')]);


  useEffect(() => {
    const date = moment(watch('fechaInicio'));
    const datePlusAMonth = moment(new Date()).add(2, 'M');
    const dateLessAMonth = moment(new Date()).add(-2, 'M');
    if (!date.isValid()) {
      setError(
        'fechaInicio',
        { type: 'focus', message: 'Ingrese una fecha válida' },
        { shouldFocus: true },
      );
    } else if (watch('fechaInicio') > datePlusAMonth.toDate()) {
      setError(
        'fechaInicio',
        {
          type: 'focus',
          message: 'Por favor no ingrese una fecha mayor a dos meses',
        },
        { shouldFocus: true },
      );
    } else if (watch('fechaInicio') < dateLessAMonth.toDate()) {
      setError(
        'fechaInicio',
        {
          type: 'focus',
          message: 'Por favor ingrese una fecha menor a dos meses',
        },
        { shouldFocus: true },
      );
    } else {
      clearErrors('fechaInicio');
    }
  }, [watch('fechaInicio')]);

  useEffect(() => {
    const date = moment(watch('fechaFin'));
    const datePlusAMonth = moment(new Date()).add(2, 'M');
    const dateLessAMonth = moment(new Date()).add(-2, 'M');
    if (!date.isValid()) {
      setError(
        'fechaFin',
        { type: 'focus', message: 'Ingrese una fecha válida' },
        { shouldFocus: true },
      );
    } else if (watch('fechaFin') > datePlusAMonth.toDate()) {
      setError(
        'fechaFin',
        {
          type: 'focus',
          message: 'Por favor no ingrese una fecha mayor a dos meses',
        },
        { shouldFocus: true },
      );
    } else if (watch('fechaFin') < dateLessAMonth.toDate()) {
      setError(
        'fechaFin',
        {
          type: 'focus',
          message: 'Por favor ingrese una fecha menor a dos meses',
        },
        { shouldFocus: true },
      );
    } else {
      clearErrors('fechaFin');
    }
  }, [watch('fechaFin')]);

  useEffect(() => {
    const date = moment(watch('fechaReporte'));
    const datePlusAMonth = moment(new Date()).add(2, 'M');
    const dateLessAMonth = moment(new Date()).add(-2, 'M');
    if (!date.isValid()) {
      setError(
        'fechaReporte',
        { type: 'focus', message: 'Ingrese una fecha válida' },
        { shouldFocus: true },
      );
    } else if (watch('fechaReporte') > datePlusAMonth.toDate()) {
      setError(
        'fechaReporte',
        {
          type: 'focus',
          message: 'Por favor no ingrese una fecha mayor a dos meses',
        },
        { shouldFocus: true },
      );
    } else if (watch('fechaReporte') < dateLessAMonth.toDate()) {
      setError(
        'fechaReporte',
        {
          type: 'focus',
          message: 'Por favor ingrese una fecha menor a dos meses',
        },
        { shouldFocus: true },
      );
    } else {
      clearErrors('fechaReporte');
    }
  }, [watch('fechaReporte')]);



  return (
    <Dialog
      className={classes.table}
      fullWidth
      maxWidth="md"
      open={isOpen}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle className={classes.title} id="form-dialog-title">
        AGREGAR Y ACTUALIZAR NOVEDAD
      </DialogTitle>
      <DialogContent>
        <Grid item container>
          <LoadingComponent key="loader" loading={scheduleState.loading} />
          <Grid container md={12}>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <CHTextFieldV2
                  label="Unidad ejecutora"
                  multiline
                  value={
                    catalogState.ExecutingUnit?.find(x => x.id === watch('idUnidadEjecutora'))
                      ?.nombre
                  }
                  isReadOnly
                />
              </Grid>
            </Grid>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <Controller
                  name="fechaReporte"
                  rules={{
                    validate: validateDate,
                  }}
                  control={control}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHTextFieldV2
                      label="Fecha De Reporte a UE"
                      onChange={onChange}
                      isReadOnly
                      error={!!error}
                      helperText={error ? error.message : null}
                      value={moment().format('DD/MM/YYYY')}
                    />
                  )}
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid container md={12}>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <Controller
                  name="fechaInicio"
                  rules={{
                    validate: validateDate,
                  }}
                  control={control}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHDatePickerV5
                      label="Fecha Inicial"
                      onChange={onChange}
                      value={
                        value.toString() === '0001-01-01T00:00:00'
                          ? new Date()
                          : value
                      }
                      error={!!error}
                      helperText={error ? error.message : null}
                      invalidDateMessage="Fecha inválida"
                    />
                  )}
                />
              </Grid>
            </Grid>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <Controller
                  name="fechaFin"
                  rules={{
                    validate: validateDate,
                  }}
                  control={control}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHDatePickerV5
                      label="Fecha Final"
                      onChange={onChange}
                      error={!!error}
                      helperText={error ? error.message : null}
                      value={
                        value.toString() === '0001-01-01T00:00:00'
                          ? new Date()
                          : value
                      }
                      invalidDateMessage="Fecha inválida"
                    />
                  )}
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid container md={12}>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <Controller
                  name="periodo"
                  control={control}
                  render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHDatePickerV2
                      label="Período"
                      onChange={onChange}
                      error={!!error}
                      helperText={error ? error.message : null}
                      views={["month", "year"]}
                      format="MM/yyyy"
                      value={value}
                      disabled={watch('idUnidadEjecutora') !== 8}
                      invalidDateMessage="Fecha inválida"
                    />
                  )}
                />
              </Grid>
            </Grid>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <CHTextFieldV2
                  label="Formato"
                  value={
                    catalogState.Formato?.find(x => x.id === watch('idFormato'))
                      ?.descripcion
                  }
                  isReadOnly
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid container md={12}>

            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <CHTextFieldV2
                  label="Medio de Envío"
                  value={
                    catalogState.MediosDeEnvio?.find(
                      x => x.id === watch('idMedioEnvio'),
                    )?.descripcion
                  }
                  isReadOnly
                />
              </Grid>
            </Grid>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <CHTextFieldV2
                  label="Segundo Formato"
                  value={
                    data?.id === 8 || data?.id === 7
                      ? catalogState.Formato?.find(x => x.id === 1)?.descripcion
                      : 'N/A'
                  }
                  isReadOnly
                />
              </Grid>
            </Grid>
          </Grid>

          <table className="table">
            <thead>
              <tr>
                <th className="fuerza">CONCEPTO NOVEDAD</th>
                <th className="unidadEje">CÓDIGO ALTA</th>
                <th className="fechaReporte">CÓDIGO BAJA</th>
              </tr>
            </thead>
            <tbody>
              {dataProduct?.map((row, index) => (
                <tr key={`table${index}`}>
                  <td className="fuerza2">
                    {
                      products?.find(
                        x => x.idConceptoNovedad === row.idConceptoNovedad,
                      )?.descripcionConceptoNovedad
                    }
                  </td>
                  <td className="unidadEje2">
                    {row.editing ? (
                      <Controller
                        name="codigoAlta"
                        control={control2}
                        render={({ field: { value, onChange } }) => (
                          <CHTextFieldV2
                            label=""
                            onChange={onChange}
                            value={value}
                            InputProps={{
                              style: {
                                fontSize: '17px',
                              },
                            }}
                          />
                        )}
                      />
                    ) : (
                      row.codigoNovedadAlta
                    )}
                  </td>
                  <td className="fechaReporte2">
                    {row.editing ? (
                      <Controller
                        name="codigoBaja"
                        control={control2}
                        render={({ field: { value, onChange } }) => (
                          <CHTextFieldV2
                            label=""
                            onChange={onChange}
                            value={value}
                            InputProps={{
                              style: {
                                fontSize: '17px',
                              },
                            }}
                          />
                        )}
                      />
                    ) : (
                      row.codigoNovedadBaja
                    )}
                  </td>
                </tr>
              ))}

              {addProduct ? (
                <tr>
                  <td style={{ width: '10%' }}>
                    <Controller
                      name="idConceptoNovedad"
                      control={control1}
                      render={({ field: { onChange, value } }) => (
                        <CHSelectV2
                          label=""
                          onChange={onChange}
                          value={value}
                          SelectDisplayProps={{
                            style: {
                              fontSize: '17px',
                              justifyContent: 'left',
                            },
                          }}
                        >
                          {products
                            .filter(
                              x =>
                                !dataProduct.find(
                                  y =>
                                    y.idConceptoNovedad === x.idConceptoNovedad,
                                ),
                            )
                            ?.map((option: any) => (
                              <MenuItem
                                key={option.idConceptoNovedad}
                                value={option.idConceptoNovedad}
                              >
                                {option.descripcionConceptoNovedad}
                              </MenuItem>
                            ))}
                        </CHSelectV2>
                      )}
                    />
                  </td>
                  <td className="unidadEje2">
                    <Controller
                      name="codigoNovedadAlta"
                      control={control1}
                      render={({ field: { value, onChange } }) => (
                        <CHTextFieldV2
                          label=""
                          onChange={onChange}
                          value={value}
                        />
                      )}
                    />
                  </td>
                  <td className="fechaReporte2">
                    <Controller
                      name="codigoNovedadBaja"
                      control={control1}
                      render={({ field: { value, onChange } }) => (
                        <CHTextFieldV2
                          label=""
                          onChange={onChange}
                          value={value}
                        />
                      )}
                    />
                  </td>
                  <td className="fechaReporte2">
                    <Button
                      className={classes.buttonAddProduct}
                      onClick={() => addProductToList()}
                    >
                      Guardar
                    </Button>
                  </td>
                </tr>
              ) : null}
            </tbody>
          </table>

          <Grid>
            <div className="buttons">
              <Button
                className={classes.buttons}
                type="button"
                onClick={handleClose}
              >
                REGRESAR
              </Button>
              <Button
                className={classes.button}
                onClick={handleSubmit(updateExecutingUnitSchedule)}
                type="submit"
              >
                ACTUALIZAR
              </Button>
            </div>
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
  );
}

export default connect()(EditSchedule);
