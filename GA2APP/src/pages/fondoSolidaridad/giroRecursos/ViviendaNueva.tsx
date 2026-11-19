import React, { useEffect, useState } from 'react';
import { useForm, UseFormReturn } from 'react-hook-form';
import {createStyles, Button, makeStyles, Grid } from '@material-ui/core';
import { toast } from 'react-toastify';
import NotificationService from '../../../services/notification.service';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import InformacionSolicitudPostulacion from './InformacionSolicitud';
import InformacionInmueblePostulacion from './InformacionInmueble';
import InformacionApoderadoAutorizadoPostulacion from './InformacionApoderadoAutorizado';
import InformacionBeneficiarioPagoPostulacion from './InformacionBeneficiarioPago';
import { MatriculaInmobiliariaModel } from '../../../models/matricula-inmobiliaria.model';
import { TerceroApoderadoPostulacion, 
         TerceroBeneficiarioPostulacion } from '../constants/fondosolidaridadform.model.type';
import { listaModalidadesGiroRecursos, listaTiposSolicitud } from '../constants/fondoSolidaridad.constants';
import ObservacionesAdicionales from './ObservacionesAdicionales';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
    }),
);

const ViviendaNueva = (props: Props) => {

    const classes = useStyles();
    const {cliente, formularioSolicitud, tipoModalidad, onClickCancelar} = props;
    const [matriculasInmobiliarias,setMatriculasInmobiliarias] = useState<MatriculaInmobiliariaModel[]>([]);
    const [apoderadosAutorizados,setApoderadosAutorizados] = useState<TerceroApoderadoPostulacion[]>([]);
    const [beneficiariosDePago,setBeneficiariosDePago] = useState<TerceroBeneficiarioPostulacion[]>([]);

    const formularioInformacionSolicitud = useForm({
        defaultValues: {
            nombres: '',
            apellidos: '',
            numeroDocumento: '',
            tipoSolicitud: '',
            modalidad: '',
            valorSolicitud: '',
            observacion: '',
        }
    })

    const formularioMatriculasInmobiliarias = useForm({
        defaultValues: {
            id: '',
            numeroMatriculaBuscar: '',
            oficinaRegistro: '',
            codigoOficina: '',
            numeroMatricula: '',
            direccion: '',
            valorSolicitud: '',
            departamentoId: '',
            ciudadId: '',
            esPrincipal: '',
        }
    })

    const formularioApoderadoAutorizado = useForm({
        defaultValues: {
            id: '',
            numeroDocumentoBuscar: '',
            tipoDocumentoId: '',
            numeroDocumento: '',
            parentesco: '',
            esAutorizado: '',
            primerNombre: '',
            segundoNombre: '',
            primerApellido: '',
            segundoApellido: '',
        }
    })

    const formularioBeneficiarioPago = useForm({
        defaultValues: {
            id: '',
            beneficiario: '',
            tipoCuenta: '',
            entidad: '',
            numeroCuenta: '',
            confirmacionNumeroCuenta: '',
            valorRetirar: '',
        }
    })

    useEffect(() => {
        formularioInformacionSolicitud.setValue('nombres', `${cliente?.primerNombre} ${cliente?.segundoNombre}`);
        formularioInformacionSolicitud.setValue('apellidos',  `${cliente?.primerApellido} ${cliente?.segundoApellido}`);
        formularioInformacionSolicitud.setValue('numeroDocumento', cliente?.identificacion);
        const descripcionTipoSolicitud = listaTiposSolicitud.find(item => item.id === formularioSolicitud.watch('tipoSolicitud'));
        formularioInformacionSolicitud.setValue('tipoSolicitud', descripcionTipoSolicitud?.descripcion || '');
        const descripcionModalidad = listaModalidadesGiroRecursos.find(item => item.id === tipoModalidad);
        formularioInformacionSolicitud.setValue('modalidad', descripcionModalidad?.descripcion || '');
        window.scrollTo(1000,0);
    },[cliente])

    const guardar = async(data: any) => {

        if(matriculasInmobiliarias.length === 0){
            toast('Debe registrar por lo menos una Matrícula Inmobiliaria', { type: toast.TYPE.ERROR });
            return;
        }

        if(apoderadosAutorizados.length === 0){
            toast('Debe registrar por lo menos un Apoderado Autorizado', { type: toast.TYPE.ERROR });
            return;
        }

        if(beneficiariosDePago.length === 0){
            toast('Debe registrar por lo menos un Beneficiario', { type: toast.TYPE.ERROR });
            return;
        }
        
        await NotificationService.genericAlert(
            'Guardado!',
            'Información almacenada',
        );
    }

    return (
        <>
            <CHPageTitleV2
				topText="Gestión de Giro de Recursos"
				title1="Vivienda Nueva"
                title2='Fondo Solidaridad'
				icono="fact_check"
			/> 
            <InformacionSolicitudPostulacion
                formulario={formularioInformacionSolicitud}
            />
            <InformacionInmueblePostulacion
                formulario={formularioMatriculasInmobiliarias}
                setMatriculasInmobiliarias={setMatriculasInmobiliarias}
            />
            <InformacionApoderadoAutorizadoPostulacion
                formulario={formularioApoderadoAutorizado}
                setApoderadosAutorizados={setApoderadosAutorizados}
            />
            <InformacionBeneficiarioPagoPostulacion
                formulario={formularioBeneficiarioPago}
                apoderadosAutorizados={apoderadosAutorizados}
                setBeneficiariosDePago={setBeneficiariosDePago}
            />
            <ObservacionesAdicionales
                formulario={formularioInformacionSolicitud}
            />
            <Grid container>
                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        onClick={formularioInformacionSolicitud.handleSubmit(guardar)}
                    >
                       GUARDAR
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        onClick={onClickCancelar}
                    >
                       REGRESAR
                    </Button>
                </Grid>
            </Grid>
          
        </>
    )
}

interface Props {
    cliente: any;
    tipoModalidad: number;
    formularioSolicitud: UseFormReturn<any>;
    onClickCancelar: () => void;
}

export default ViviendaNueva;