import React, { useEffect, useState } from 'react';
import { makeStyles, createStyles } from '@material-ui/core';
import { connect } from 'react-redux';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import DatosPersonales from '../components/datos-personales/datosPersonales.component';
import DatosAportesCategoriaCliente from '../components/aportes-por-categoria-cliente/aportes-por-categoria-cliente.component';
import { AppState } from '../../../store/rootReducer';
import { ClientState } from '../../../reducers/client.reducer';
import DetalleCuentascliente from '../components/detalle-cuentas-cliente/detalle-cuentas-cliente.component';
import { GetCausalesBloqueoType } from '../../../types/catalog.actions.type';
import { startGetAllCausalesBloqueoCuenta } from '../../../actions/catalog.action';
import { CatalogState } from '../../../reducers/catalog.reducer';


const useStyles = makeStyles(() =>
  createStyles({
    root: {
      flexGrow: 1,
    }
  }),
);

const controlAdministracionCuentas = (props: Props) => {

  const { clientState, catalogState, startGetAllCausalesBloqueoCuenta } = props;

  const classes = useStyles();
  const [objetoRespuesta, setObjetoRespuesta] = useState<any>(clientState?.admincuentas);

  const actualizarObjetoRespuesta = (data: any) => {
    setObjetoRespuesta(data)
  }

  useEffect(() => {
    startGetAllCausalesBloqueoCuenta();
  }, []);

  return (
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Administración Cuentas'
        topTextNivel2='Detalle De Cuentas'
      />

      <IconTitle
        title='Detalle De'
        subTitle='Cuentas'
        icon='fch-Productos-clientes-0-1'
      />

      <DatosPersonales responseAdminCuenta={clientState.admincuentas} />
      {(clientState.admincuentas.tipoAfiliacionId === 1) && <DatosAportesCategoriaCliente responseAdminCuenta={objetoRespuesta} />}
      <DetalleCuentascliente
        responseAdminCuenta={clientState.admincuentas}
        tipoBloqueo={catalogState.causalesBloqueoTypes}
        actualizarObjetoRespuesta={actualizarObjetoRespuesta} tiposIdentificacion={[]}
      />
    </div>

  );
}

interface LinkMapStateToProps {
  tipoCausalBloqueo: GetCausalesBloqueoType;
  clientState: ClientState;
  catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
  clientState: state.client,
  catalogState: state.catalog,
});

interface LinkMapDispatchToProps {
  startGetAllCausalesBloqueoCuenta: () => void;
}

const mapDispatchToProps = {
  startGetAllCausalesBloqueoCuenta
};


type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(controlAdministracionCuentas);




