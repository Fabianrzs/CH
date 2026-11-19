import React, { useEffect, useState } from 'react';
import { createMuiTheme, createStyles, createTheme, Grid, makeStyles, MenuItem, MuiThemeProvider } from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { ENTIDADBANCARIA, IDENTIFICATIONTYPEDTO, VALORCATALOGO } from '../../../../types/catalog.type';

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

function TerceroBeneficiarioDesafiliaciones({
	responseSolicitud,
	controlBeneficiario,
	listaValorCatalogo,
	listaEntidadBancaria,
	obtenerTipoCuenta
}: {
	controlBeneficiario: any,
	responseSolicitud: ResponseSeveranceModel | undefined,
	listaValorCatalogo: VALORCATALOGO[];
	listaEntidadBancaria: ENTIDADBANCARIA[];
	obtenerTipoCuenta: (catalogoId: number) => void
}) {
	const classes = useStyles();
	const [readOnlyEntidad, setReadOnlyEntidad] = useState<boolean>(false);
	const [readOnlyNumeroCuenta, setReadOnlyNumeroCuenta] = useState<boolean>(false);
	const [tipo, setTipo] = useState<string>('');

	function tipoIdentificacion(dato: number) {
		if (dato === 0) {
			setTipo('CEDULA DE CIUDADANIA');
			controlBeneficiario.setValue('tipoDocObj', 'CEDULA DE CIUDADANIA');
		} else if (dato === 1) {
			setTipo('CEDULA DE EXTRANJERIA');
			controlBeneficiario.setValue('tipoDocObj', 'CEDULA DE EXTRANJERIA');
		}
	}

    const formLabelsTheme = createTheme({
        overrides: {
            MuiFormLabel: {
                asterisk: {
                    color: '#db3131',
                    '&$error': {
                        color: '#db3131',
                    },
                },
            },
        },
    });

	useEffect(() => {
		obtenerTipoCuenta(67);
		controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
		controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' });
		controlBeneficiario.setValue('razonSocial', String(responseSolicitud?.apellidosCliente));
		controlBeneficiario.setValue('numDoc', String(responseSolicitud?.identificacionCliente));
		tipoIdentificacion(Number(responseSolicitud?.tipoIdentificacion));
	}, []);

	return (
		<CHPaper title="Información Beneficiario de Pago">
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="tipoDocObj"
						control={controlBeneficiario.control}
						rules={{
							required: 'Este campo es requerido.',
						}}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<MuiThemeProvider theme={formLabelsTheme}>
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly
									label="Tipo de Identificación"
									value={value || tipo}
									onChange={onChange}
								/>
							</MuiThemeProvider>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="numDoc"
						control={controlBeneficiario.control}
						rules={{
							required: 'Este campo es requerido.',
						}}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<MuiThemeProvider theme={formLabelsTheme}>
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly
									label="Numero de documento"
									value={value || String(responseSolicitud?.identificacionCliente)}
									onChange={onChange}
									inputProps={{ maxLength: 12 }}
								/>
							</MuiThemeProvider>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="razonSocial"
						control={controlBeneficiario.control}
						rules={{
							required: 'Este campo es requerido.',
						}}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<MuiThemeProvider theme={formLabelsTheme}>
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly
									label="Nombre / Razón Social"
									value={value || String(responseSolicitud?.apellidosCliente)}
									onChange={onChange}
									InputLabelProps={{ shrink: true }}
									inputProps={{maxLength: 50}}
								/>
							</MuiThemeProvider>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="tipoCuentaObj"
						control={controlBeneficiario.control}
						rules={{
							required: 'Este campo es requerido.',
						}}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<MuiThemeProvider theme={formLabelsTheme}>
								<CHSelect
									error={!!error}
									label="Tipo de Cuenta *"
									value={value}
									helperText={error ? error.message : null}
									onChange={value => {
										const dato = value.target
											.value as string;
										const idEntidadBancaria =
											dato.split('_')[0];
										if (idEntidadBancaria === '322') {
											setReadOnlyEntidad(true);
											setReadOnlyNumeroCuenta(true);
											controlBeneficiario.setValue('entidadObj', '');
											controlBeneficiario.setValue('numeroCuenta', '');
											controlBeneficiario.register('entidadObj', { required: false });
											controlBeneficiario.register('numeroCuenta', { required: false });
											controlBeneficiario.clearErrors('entidadObj');
											controlBeneficiario.clearErrors('numeroCuenta');
										} else {
											setReadOnlyEntidad(false);
											setReadOnlyNumeroCuenta(false);
											controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
											controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' });
										}
										onChange(value);
									}}
								>
									{listaValorCatalogo?.map(
										(option: VALORCATALOGO) => {
											const dato = `${option.idValor}_${option.descripcion}`;
											return (
												<MenuItem
													key={option.idValor}
													value={dato}
												>
													{option.descripcion}
												</MenuItem>
											);
										},
									)}
								</CHSelect>
							</MuiThemeProvider>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="entidadObj"
						control={controlBeneficiario.control}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<MuiThemeProvider theme={formLabelsTheme}>
								<CHSelect
									error={!!error}
									label="Entidad Bancaria *"
									readOnly={readOnlyEntidad}
									value={value}
									helperText={error ? error.message : null}
									onChange={value => {
										onChange(value);
									}}
								>
									{listaEntidadBancaria?.map(
										(option: ENTIDADBANCARIA) => {
											const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
											return (
												<MenuItem
													key={
														option.idEntidadBancaria
													}
													value={dato}
												>
													{option.nombreRazonSocial}
												</MenuItem>
											);
										},
									)}
								</CHSelect>
							</MuiThemeProvider>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="numeroCuenta"
						control={controlBeneficiario.control}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<MuiThemeProvider theme={formLabelsTheme}>
								<CHTextField
									numberMask
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={readOnlyNumeroCuenta}
									InputLabelProps={{ shrink: true }}
									label="Número"
									value={value}
									onChange={onChange}
								/>
							</MuiThemeProvider>
						)}
					/>
				</Grid>
			</Grid>
		</CHPaper>
	);
}

export default TerceroBeneficiarioDesafiliaciones;
