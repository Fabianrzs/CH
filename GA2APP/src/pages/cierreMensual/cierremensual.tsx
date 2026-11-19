import React, { useEffect } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles, MenuItem } from "@material-ui/core";
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { Controller, useForm } from 'react-hook-form';
import ContainerTitle from '../../components/containe-title/container-title';
import TituloIcono from '../solicitudes/components/titulo-icono/tituloIcono.component';
import CHSelect from '../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../components/loading/loading.component';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import { CierreMensualModel } from '../../types/cierre-mensual.model.type';
import { startSetCierreMensual, startgetIPC } from '../../actions/cierreMensual.action';
import { AppState } from '../../store/rootReducer';
import { CatalogState } from '../../reducers/catalog.reducer';

import CHTextField from '../../components/ch-text-field/CHTextField';
import history from '../../utils/history.router';


const useStyles = makeStyles(() =>
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
    }
  }),
);


const CierreMensual = (props: Props) => {

  const meses = [
    'Enero',
    'Febrero',
    'Marzo',
    'Abril',
    'Mayo',
    'Junio',
    'Julio',
    'Agosto',
    'Septiembre',
    'Octubre',
    'Noviembre',
    'Diciembre',
  ];

  const { startSetCierreMensual, startgetIPC, catalogState } = props;
  const classes = useStyles();
  const { control, handleSubmit } = useForm({
    defaultValues: {
      tipoProceso: '',
      generaActualizacion: '',

    },
  });



  useEffect(() => {
    startgetIPC();

  }, []);

  const cancelar = () => {
    history.goBack();
  }

  const onSubmit = async (data: any) => {

    data.generaActualizacion = generaActualizacionstatus(data.generaActualizacion);
    data as CierreMensualModel

    if (data.generaActualizacion) {

      swal({
        text: "Este proceso actualizará las cuentas. ¿Desea continuar?",
        icon: "warning",
        buttons: ["Cancelar", "Aceptar"]
      }).then(respuesta => {
        if (respuesta) {
          startSetCierreMensual(data);

        }
      })

    } else {

      startSetCierreMensual(data);
    }


  }

  const generaActualizacionstatus = (value: string) => {

    if (value === "true") {
      return true;
    }
    return false
  }

  return (<div className={classes.root}>

    <LoadingComponent key="loader" loading={false} />

    <PageIndicators
      topTextNivel1='Cierre mensual'
      topTextNivel2='Cierre mensual' />

    <IconTitle
      title='Cierre'
      subTitle='Mensual'
      icon='fch-Buscar-0-1'
    />

    <Grid >
      <Grid item xs={12}>
        <Card>
          <ContainerTitle
            title="Cierre mensual – cálculo de intereses"
          />
          <CardContent>


            <Grid container justifyContent="space-around">
              <Grid item xs={3}>
                <TituloIcono
                  title1="Fecha"
                  subtitle="Cierre"
                  icono="event_available"
                />
              </Grid>
              <Grid item xs={6} >

                <CHTextField
                  isReadOnly={false}
                  label="FECHA CIERRE MENSUAL"
                  value={`${meses && catalogState.ipc && catalogState.ipc.mes !== undefined
                      ? meses[catalogState.ipc.mes].toString()
                      : ''
                    } del ${catalogState.ipc && catalogState.ipc.anio
                      ? catalogState.ipc.anio
                      : ''
                    }`}

                />

              </Grid>
            </Grid>
            <br />
            <br />


            <Grid container justifyContent="space-around">
              <Grid item xs={3}>
                <TituloIcono
                  title1="Tasa"
                  subtitle="IPC"
                  icono="event_available"
                />
              </Grid>
              <Grid item xs={6} >

                <CHTextField
                  isReadOnly={false}
                  label="VALOR TASA IPC"
                  value={catalogState.ipc ? catalogState.ipc.valorIPC : ''}
                />

              </Grid>
            </Grid>

            <br />
            <br />
            <Grid container justifyContent="space-around">
              <Grid item xs={3}>
                <TituloIcono
                  title1="Concepto"
                  subtitle=""
                  icono="event_available"
                />
              </Grid>
              <Grid item xs={6} >
                <Controller
                  name="tipoProceso"
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
                      label="CONCEPTO"
                      value={value}
                      helperText={error ? error.message : null}
                      onChange={onChange}
                    >

                      <MenuItem key={1} value={1}>
                        Ahorros
                      </MenuItem>

                      <MenuItem key={2} value={2}>
                        Cesantías
                      </MenuItem>

                      <MenuItem key={3} value={3}>
                        Ahorros y Cesantías
                      </MenuItem>

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
                  title1="Aplica a"
                  subtitle="Cuentas"
                  icono="event_available"
                />
              </Grid>
              <Grid item xs={6} >
                <Controller
                  name="generaActualizacion"
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
                      label="APLICA A CUENTAS"
                      value={value}
                      helperText={error ? error.message : null}
                      onChange={onChange}
                    >

                      <MenuItem key={1} value="true">
                        Si
                      </MenuItem>

                      <MenuItem key={2} value="false">
                        No
                      </MenuItem>




                    </CHSelect>
                  )}
                />
              </Grid>
            </Grid>
            <br />
            <br />
          </CardContent>
        </Card>
      </Grid>
      <div className="buttonssolicitud">
        <Button
          className={classes.buttonsecundario}
          variant="outlined"
          onClick={cancelar}
        >
          CANCELAR
        </Button>

        <Button
          className={classes.buttonprincipal}
          variant="outlined"
          onClick={handleSubmit(onSubmit)}
        >
          ACEPTAR
        </Button>
      </div>
    </Grid>

  </div>
  )
}


interface LinkMapStateToProps {
  catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
  catalogState: state.catalog,
});


interface LinkMapDispatchToProps {
  startSetCierreMensual: (data: CierreMensualModel) => void;
  startgetIPC: () => void;
}

const mapDispatchToProps = {
  startSetCierreMensual,
  startgetIPC
};



type Props = LinkMapDispatchToProps & LinkMapStateToProps;

export default connect(mapStateToProps, mapDispatchToProps)(CierreMensual);
