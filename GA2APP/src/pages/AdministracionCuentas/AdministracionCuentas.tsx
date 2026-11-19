import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { Button, Card, CardContent, Grid, MenuItem, makeStyles, createStyles } from '@material-ui/core';
import CHSelect from '../../components/ch-select/CHSelect';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import ContainerTitle from '../../components/containe-title/container-title';
import CHTextField from '../../components/ch-text-field/CHTextField';
import TituloIcono from './components/titulo-icono/tituloIcono.component'
import { AppState } from '../../store/rootReducer';
import {
  startGetAllTypeDocuments
} from '../../actions/catalog.action';
import { CatalogState } from '../../reducers/catalog.reducer';
import { IDENTIFICATIONTYPEDTO } from '../../types/catalog.type';

import { startGetAdministracionCuentasAfiliado } from '../../actions/client.action'
import { AdministracionParametroModel } from '../../models/requests/create-simulation-client';
import { LoadingComponent } from '../../components/loading/loading.component';
import { ClientState } from '../../reducers/client.reducer';
import history from '../../utils/history.router';
import AuthService from '../../services/auth.service';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { Auditoria } from '../../types/Auditoria.model.type';

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
    },
    marginContainer: {
      marginTop: '2rem',
    }
  }),
);

const AdministracionCuentas = (props: Props) => {

  const { catalogState, clientState, startGetAdministracionCuentasAfiliado, startGetAllTypeDocuments } = props;

  const { control, handleSubmit } = useForm({
    defaultValues: {
      tipoDocumentoId: '',
      numeroDocumento: '',
      typerequest: ''
    },
  });

  useEffect(() => {
    startGetAllTypeDocuments();

  }, []);


  const classes = useStyles();

  const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
  const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
  const catalogApi = CatalogApi();

  const isValidIPv4 = (ip: string) => {
    const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    return ipv4Regex.test(ip);
  };

  const isValidIPv6 = (ip: string) => {
    const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
    return ipv6Regex.test(ip);
  };

  const enviarIp = async (ip: any) => {
    const convertIp = ip.split('.')[0]

    const hexadecimal = convertIp
    await catalogApi
      .getObtenerIpLocal(hexadecimal)
      .then(async (response: any) => {

        if (response) {
          if (response.isSuccess) {
            setPublicIpAddress(response.data);
          }
        }
      })
      .catch(e => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
      });

  }

  useEffect(() => {
    const fetchLocalIPAddress = () => {
      const peerConnection = new RTCPeerConnection({ iceServers: [] });
      peerConnection.createDataChannel('');
      peerConnection.createOffer()
        .then((offer) => peerConnection.setLocalDescription(offer))
        .catch((error) => console.error('Error creating offer:', error));

      peerConnection.onicecandidate = (event) => {
        if (event.candidate) {
          const ip: any = event.candidate.address;
          if (isValidIPv4(ip) || isValidIPv6(ip)) {
            setLocalIpAddress(ip);
            enviarIp(ip);
          } else {
            setLocalIpAddress(ip);
            enviarIp(ip);
          }
        }
      };

      return () => {
        peerConnection.close();
      };
    };
    fetchLocalIPAddress();

  }, []);

  const onSubmit = async (data: any) => {

    startGetAdministracionCuentasAfiliado(data);

    const auditoriaApi = AuditoriaApi();
    const user = AuthService.getUserInfo();
    const nav = window.navigator as any;
    const usrAg = nav.userAgentData.brands.find(
      (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    )?.brand;

    let descripcionTipoDoc = 'No especificado';

        if (parseInt(data.tipoDocumentoId, 10) === 1) {
            descripcionTipoDoc = 'CÉDULA DE CIUDADANÍA';
        } else if (parseInt(data.tipoDocumentoId, 10) === 3) {
            descripcionTipoDoc = 'CÉDULA DE EXTRANJERÍA';
        } else if (parseInt(data.tipoDocumentoId, 10) === 8) {
            descripcionTipoDoc = 'CÓDIGO JUZGADO';
        } else if (parseInt(data.tipoDocumentoId, 10) === 9) {
            descripcionTipoDoc = 'CÓDIGO MILITAR';
        } else if (parseInt(data.tipoDocumentoId, 10) === 2) {
            descripcionTipoDoc = 'NÚMERO DE IDENTIFICACIÓN TRIBUTARIA';
        } else if (parseInt(data.tipoDocumentoId, 10) === 5) {
            descripcionTipoDoc = 'NÚMERO ÚNICO DE IDENTIFICACIÓN';
        } else if (parseInt(data.tipoDocumentoId, 10) === 6) {
            descripcionTipoDoc = 'PASAPORTE';
        } else if (parseInt(data.tipoDocumentoId, 10) === 7) {
            descripcionTipoDoc = 'REGISTRO CIVIL';
        } else if (parseInt(data.tipoDocumentoId, 10) === 4) {
            descripcionTipoDoc = 'TARJETA DE IDENTIDAD';
        }

    const requestAuditoria = {
      navegador: usrAg,
      sistemaOperativo: nav.userAgentData.platform,
      nombreUsuario: user.USR_USERNAME,
      usurioId: user.USR_ID,
      ip: publicIpAddress,
      modulo: 'Búsqueda De Cuentas De Usuario',
      hostname: document.location.host,
      tipoAccion: 'CONSULTAS',
      idTipoAccion: 3,
      descripcion: `Consulta de cuentas de usuario con los siguientes filtros:
                      - Tipo de documento: ${descripcionTipoDoc || 'No especificada'}
                      - Número de documento: ${data.numeroDocumento || 'No especificada'}`,
    } as Auditoria;

    auditoriaApi.InsertAuditoria(requestAuditoria);
  }

  const onCancel = () => {
    history.push('/Inicio');
  }

  return (

    <div className={classes.root}>

      <LoadingComponent key="loader" loading={clientState.loading} />

      <PageIndicators
        topTextNivel1='Administración Cuentas'
        topTextNivel2='Búsqueda De Cuentas' />

      <IconTitle
        title='Búsqueda De Cuentas'
        subTitle='De Usuario'
        icon='fch-Buscar-0-1'
      />

      <Grid >
        <Grid item xs={12}>
          <Card>
            <ContainerTitle
              title="Administración de Usuarios"
            />
            <CardContent>

              <Grid container justifyContent="space-around">
                <Grid item xs={3}>
                  <TituloIcono
                    title1="Tipo"
                    subtitle="de documento"
                    icono="event_available"
                  />
                </Grid>
                <Grid item xs={6} >
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
                        label="TIPO DE DOCUMENTO"
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
              </Grid>
              <br />
              <br />
              <Grid container justifyContent="space-around">
                <Grid item xs={3}>
                  <TituloIcono
                    title1="Número"
                    subtitle="de documento"
                    icono="event_available"
                  />
                </Grid>
                <Grid item xs={6} >
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
                        numberMask
                        error={!!error}
                        helperText={error ? error.message : null}
                        isReadOnly={false}
                        label="Número de Documento"
                        value={value}
                        onChange={onChange}
                      />
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
            onClick={onCancel}
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

export default connect(mapStateToProps, mapDispatchToProps)(AdministracionCuentas);
