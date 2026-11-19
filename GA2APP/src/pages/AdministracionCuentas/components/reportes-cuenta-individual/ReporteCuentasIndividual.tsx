import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { Button, Card, CardContent, Grid, MenuItem, makeStyles, createStyles, Checkbox, ListItemText, Chip, Select, Input, withStyles, Theme} from '@material-ui/core';
import moment from 'moment';
import { toast } from 'react-toastify';
import { CheckBox } from '@material-ui/icons';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import ContainerTitle from '../../../../components/containe-title/container-title';
import TituloIcono from '../titulo-icono/tituloIcono.component';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { CONCEPTOTYPEDTO, IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { startGetAdministracionCuentasAfiliado } from '../../../../actions/client.action'
import {startGetAllTypeDocuments} from '../../../../actions/catalog.action';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { ClientState } from '../../../../reducers/client.reducer';
import { AppState } from '../../../../store/rootReducer';
import { AdministracionParametroModel } from '../../../../models/requests/create-simulation-client';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import afiliadoApi from '../../../../services/api-afiliado.service';
import CatalogApi from '../../../../services/api-catalog.service';


const useStyles = makeStyles((theme:any) =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center',
    },
    buttonprincipal: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#FEC917'
    },
    buttonsecundario: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#43b5f1'
    },
    processButton: {
      backgroundColor: '#1C3F8B',
      color: '#fff',
      margin: '8px 0',
    },
    root: {
      flexGrow: 1,
    },
    marginContainer:{
      marginTop: '2rem',
    },
    formControl: {
      margin: theme.spacing(1),
      minWidth: 120,
      maxWidth: 300,
    },
    chips: {
      display: 'flex',
      flexWrap: 'wrap',
    },
    chip: {
      margin: 2,
    },
    noLabel: {
      marginTop: theme.spacing(3),
    },
  }),
);
const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;

const MenuProps: any = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250,
    },
  },
};



const ReporteCuentaIndividual = (props: Props) => {

  const { catalogState, clientState, startGetAdministracionCuentasAfiliado, startGetAllTypeDocuments } = props;
  const classes = useStyles();
  const catalogApi = CatalogApi()

  const { control, handleSubmit, watch, setValue, getValues } = useForm({
    defaultValues: {
        nombreReporte: '',
        tipoConceptos:'',
        fechaInicio: '',
        fechaFin: ''
    },
  });

  const [ConceptosReporte,setConceptosReporte] = useState([]);
 const [personName, setPersonName] = React.useState<string[]>([]);

  useEffect(() => {
    startGetAllTypeDocuments();
    ObtenerTipoConceptosReporte();
  }, []);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    setPersonName(event.target.value as string[]);
  };

  const onSubmit = async (data: any) => {
    const { tipoConceptos } = data;
    const idsConceptos = tipoConceptos.map(({ id } : any) => id);
    const request = {
      fechaInicial: moment(`${data.fechaInicio}`).format('YYYY-MM-DD'),
      fechaFinal: moment(`${data.fechaFin}`).format('YYYY-MM-DD'),
      idConceptos: idsConceptos,
      idReporte: data.nombreReporte
    }
    
    console.log("request", request);
  }

  const ObtenerTipoConceptosReporte = async() => {
    await catalogApi
    .getTipoConceptosReporte()
    .then(async (response: any) => {
        if (response) {
            if (response.isSuccess) {
                setConceptosReporte(response.data)       
            }
        }
    })
    .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
    });
}

  const listaReportes = [
    { id: 1, descripcion: "Conciliación Contable" },
    { id: 2, descripcion: "Consolidado General de Pagos" }, 
  ];

  const validateEndDate = (value: string) => {
    const date = moment(value);
    let message = '';
    if (date > moment()) {
        message = "La fecha debe ser menor a la fecha actual";
    } else if (date < moment(watch('fechaInicio'))) {
        message = "La fecha debe ser menor a la fecha inicial";
    }

    return message !== '' ? message : true;
};
const validateDate = (value: string) => {
    const date = moment(value);
    let message = '';
    if (date > moment()) {
        message = "La fecha debe ser menor a la fecha actual";
    }

    return message !== '' ? message : true;
};

const today = new Date();


  return (

    <div className={classes.root}>

      <LoadingComponent key="loader" loading={clientState.loading} />

      <PageIndicators
        topTextNivel1='Administración Cuentas'
        topTextNivel2='Reportes Cuentas Individuales' />

      <IconTitle
        title='Generación de Reportes '
        subTitle='Cuentas Individuales'
        icon='fch-Buscar-0-1'
      />

      <Grid >
        <Grid item xs={12}>
          <Card>
            <ContainerTitle
              title="Consulta Conceptos"

            />
            <CardContent>

              <Grid container justifyContent="space-around">
                <Grid item xs={3}>
                  <TituloIcono
                    title1="Tipo"
                    subtitle="de Reporte"
                    icono="fact_check"
                  />
              </Grid>
                <Grid item xs={3}>
                    <Controller
                        name="nombreReporte"
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
                              label="Documentos"
                              value={value}
                              helperText={error ? error.message : null}
                              onChange={onChange}
                          >
                            {listaReportes.map((option: any) => (
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

                    <Grid item xs={3}>
                    <Controller
                        name="tipoConceptos"
                        control={control}
                        rules={{
                        required: 'Este campo es requerido.',
                        }}
                        render={({
                        field: { onChange, value },
                        fieldState: { error },
                        }) => (
                    <CHSelect
                        label="Conceptos"
                        multiple
                        value={personName}
                        onChange={e => {
                          handleChange(e)
                          onChange(e)
                        }}
                        input={<Input id="select-multiple-chip" />}
                        renderValue={(selected) => (

                          <div className={classes.chips}>
                            {(selected as any[]).map((value) => (
                              <Chip key={value.id} label={value.descripcion}  className={classes.chip} />
                            ))}
                          </div>
                        )}
                        MenuProps={MenuProps}
                      >
                        {ConceptosReporte.map((name:any) => (
                          <MenuItem key={name?.id} value={name}>
                            {name.descripcion}
                          </MenuItem>
                        ))}
                  </CHSelect>
                  )}
                  />
                </Grid>
              </Grid>

    
              <br />
              <br />
              <Grid container justifyContent="space-around">
                <Grid item xs={3}>
                  <TituloIcono
                    title1="Fechas"
                    subtitle="de reporte"
                    icono="date_range"
                  />
                </Grid>
              <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaInicio"
                                rules={{
                                    validate: validateDate
                                }}
                                control={control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : ""}
                                        label="Fecha Inicial"
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
                                name="fechaFin"
                                control={control}
                                rules={{
                                    validate: validateEndDate
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : ""}
                                        label="Fecha Final"
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
                        </Grid>

              <br />
              <br />
            <div className="buttonssolicitud">

                <Button
                className={classes.buttonprincipal}
                variant="outlined"
                onClick={handleSubmit(onSubmit)}
                >
                GENERAR
                </Button>
            </div>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </div>

  );

}


interface LinkMapStateToProps {
  catalogState: CatalogState;
  clientState: ClientState;
}

const mapStateToProps = (state: AppState) => ({
  catalogState: state.catalog,
  clientState: state.client
});

interface LinkMapDispatchToProps {
  startGetAdministracionCuentasAfiliado: (data: AdministracionParametroModel) => void;
  startGetAllTypeDocuments: () => void;
}

const mapDispatchToProps = {
  startGetAdministracionCuentasAfiliado,
  startGetAllTypeDocuments
  
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ReporteCuentaIndividual);
