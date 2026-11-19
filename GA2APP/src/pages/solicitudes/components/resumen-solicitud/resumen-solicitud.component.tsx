import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { toast } from 'react-toastify';
import AttachFileIcon from '@material-ui/icons/AttachFile'; // Importa correctamente el ícono

import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import CHTextFieldIcon from '../../../../components/ch-text-field-icon/CHTextFieldIcon';
import asigTareas from '../../../../services/api-asignacionTareas.service';
import AuthService from '../../../../services/auth.service';
import { GUID_DEFAULT } from '../../../../constants/constants';
import NotificationService from '../../../../services/notification.service';
import SolicitudApi from '../../../../services/api-solicitud.service';



const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
		},
		column2: {
			padding: '10px',
			textAlign: 'left',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnButtonAdd: {
			textAlign: 'right',
		},
		columnLabel: {
			padding: '10px',
		},
	}),
);

function ResumenSolicitud({
	responseSolicitud,
	observaciones,
}: {
	responseSolicitud: ResponseSeveranceModel | undefined;
	observaciones: string;
}) {

	const apiSolicitud = SolicitudApi();

	const [linkDodo, setLinkDodo] = useState('')
	const [descripcionProceso, setDescripcionProceso] = useState('');


	useEffect(()=> {
		obtenerUrlDodo();
		tipoSolicitud();
	},[])


	const obtenerUrlDodo = async() => {
	
      
        const radicado= responseSolicitud?.numeroRadicadoSG;
        
        await apiSolicitud
            .GetBoteneEnlaceDodo(String(radicado))
            .then(async respuesta => {
                setLinkDodo(respuesta.data);
          
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

	}

	
	const tipoSolicitud = async () => {
        if (
            responseSolicitud?.nombreTarea?.endsWith('V 14') &&
            responseSolicitud?.descripcionSubModalidad === 'PRIMER PAGO'
        ) {
            setDescripcionProceso('SOLICITUD VIVIENDA 14');
        } else {
            setDescripcionProceso(responseSolicitud?.descripcionProceso || '');
        }
    };
    
	


	const classes = useStyles();
	
	return (
		<>
			<CHPaper title="Información solicitud">
				<Grid container>
					<Grid container>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Número de solicitud"
								value={responseSolicitud?.numeroSolicitud}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Nombres"
								value={responseSolicitud?.nombresCliente || ''}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Apellidos"
								value={responseSolicitud?.apellidosCliente || ''}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
							    numberMask
								isReadOnly
								label="Número De Documento"
								value={
									responseSolicitud?.identificacionCliente || ''
								}
							/>
						</Grid>
					
					</Grid>
					<Grid container>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Tipo Solicitud"
								value={descripcionProceso || ''}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Modalidad"
								value={
									responseSolicitud?.descripcionModalidad || ''
								}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Sub-Modalidad"
								value={
									responseSolicitud?.descripcionSubModalidad || ''
								}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								currencyMask
								isReadOnly
								label="Valor Solicitud"
								value={responseSolicitud?.valorRetirar || ''}
							/>
						</Grid>

					</Grid>
					<Grid container>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Dirección"
								value={responseSolicitud?.direccionCliente || ''}
							/>
						</Grid>
						
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Teléfono"
								value={responseSolicitud?.telefonoCliente || ''}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Celular"
								value={responseSolicitud?.celularCliente || ''}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Correo Electrónico"
								value={responseSolicitud?.correoCliente || ''}
							/>
						</Grid>
					</Grid>
					<Grid container>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Fecha De Inicio"
								value={
									moment(
										responseSolicitud?.fechaSolicitud,
									).format('DD/MM/YYYY') || ''
								}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Fecha Final"
								value={
									moment(
										responseSolicitud?.fechaSolicitud,
									).format('DD/MM/YYYY') || ''
								}
							/>
						</Grid>
					
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextFieldIcon
								isReadOnly
								label="Número De Radicado"
								icon={(
									<a  href={linkDodo} target="_blank" rel="noopener noreferrer">
										
										<AttachFileIcon />
									</a>
								)}
								value={responseSolicitud?.numeroRadicadoSG || ''}
							/>
						</Grid>
				
					</Grid>
					


				</Grid>
			</CHPaper>
		</>
	);
}

export default ResumenSolicitud;
