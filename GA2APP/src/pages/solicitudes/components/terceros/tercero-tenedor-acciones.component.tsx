import React, { useEffect } from 'react';
import { createStyles,  Grid, makeStyles, MenuItem  } from '@material-ui/core';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHPaper from "../../../../components/ch-paper/CHPaper";
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

function TerceroTenedorAcciones({
	listaTipoDocumentos,
	responseSolicitud,
	formulario,
	obtenerCiudadPorVendedorId,
	siEsAprobadaSolcitud,
	campoSoloLecturaRadicacionAprobada
}: {
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[],
	responseSolicitud: ResponseSeveranceModel | undefined;
	formulario: UseFormReturn<any>;
	obtenerCiudadPorVendedorId: (stateID?: string) => void;
	siEsAprobadaSolcitud: boolean;
	campoSoloLecturaRadicacionAprobada: boolean;
}) {
	const classes = useStyles();
	
	useEffect(() => {
		
		if (responseSolicitud?.solicitudTerceroTenedorAccionesDto !== null) {
			obtenerCiudadPorVendedorId(responseSolicitud?.solicitudTerceroTenedorAccionesDto.departamento);
			formulario.setValue("tipoDoc", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.tipoDoc));
			formulario.setValue("numDoc", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.numDoc));
			formulario.setValue("razonSocial", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto?.razonsocial));
			formulario.setValue("direccion", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.direccion));
			formulario.setValue("departamento", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.departamento));
			formulario.setValue("ciudad", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.ciudad));
			formulario.setValue("correo", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.correo));
			formulario.setValue("telefono", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.telefono));
			formulario.setValue("emisor", String(responseSolicitud?.solicitudTerceroTenedorAccionesDto.emisor));
		}
	}, []);

	return (
		<>
			<CHPaper title="Emisor de Acciones">
				<Grid container>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="tipoDoc"
							control={formulario.control}
							render={({
								field: { onChange, value },
							}) => (
								<CHSelect
									label="Tipo de Documento *"
									value={value}
									readOnly={campoSoloLecturaRadicacionAprobada || siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')}
									onChange={(value) => {
										onChange(value);
									}}
								>
									{listaTipoDocumentos?.map(
										(option: IDENTIFICATIONTYPEDTO) => {
											if(responseSolicitud?.descripcionModalidad === "ADQUISICIÓN DE ACCIONES DE PROPIEDAD DEL ESTADO"){
												if(option.descripcion === "NÚMERO DE IDENTIFICACIÓN TRIBUTARIA"){
													const dato = `${option.id}`;
													return (
														<MenuItem
															key={option.id}
															value={dato}
														>
															{option.descripcion}
														</MenuItem>
													);
												}
												
											}else{
												const dato = `${option.id}`;
												return (
													<MenuItem
														key={option.id}
														value={dato}
													>
														{option.descripcion}
													</MenuItem>
												);
											}
											return null;
											
										}
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="numDoc"
							control={formulario.control}
							render={({
								field: { onChange, value },
								
							}) => (
								<CHTextField
									numberMask
									InputLabelProps={{ shrink: true }}
									isReadOnly={campoSoloLecturaRadicacionAprobada || siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')}
									label="Número de Documento *"
									value={value}
									inputProps={{maxLength: 12}}
									onChange={onChange}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="razonSocial"
							control={formulario.control}
							render={({
								field: { onChange, value },
							}) => (
								<CHTextField
									InputLabelProps={{ shrink: true }}
									isReadOnly={campoSoloLecturaRadicacionAprobada || siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')}
									label="Nombre / Razón Social *"
									value={value}
									onChange={onChange}
									inputProps={{style: {
										textTransform: 'uppercase',
									},maxLength: 50}}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="correo"
							control={formulario.control}
							rules={{
							  pattern: {
								value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
								message: 'Correo inválido'
							  },
							}}
							render={({
							  field: { onChange, onBlur, value },
							  fieldState: { error },
							}) => (
							  <CHTextField
								InputLabelProps={{ shrink: true }}
								label="Correo Electrónico *"
								value={value}
								error={!!error}
								helperText={error?.message}
								onChange={onChange}
								onBlur={() => {
								  onBlur();
								  formulario.trigger('correo');
								}}
							  />
							)}
						  />
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="telefono"
							control={formulario.control}
							rules={{
								pattern: /\(?([3|6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
							}}
							render={({
								field: { onChange, onBlur, value },
								fieldState: { error },
							}) => (
								<CHTextField
									InputLabelProps={{ shrink: true }}
									isReadOnly={campoSoloLecturaRadicacionAprobada || siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')}
									label="Teléfono *"
									value={value}
									onChange={onChange}
									error={!!error}
									helperText={error ? 'El télefono debe comenzar por 3' : null}
									cellPhoneMask
									onBlur={() => {
										onBlur();
										formulario.trigger('telefono');
									}}
								/>
							)}
						/>
					</Grid>
				</Grid>
			</CHPaper>
			
		</>
	)
}

export default TerceroTenedorAcciones;