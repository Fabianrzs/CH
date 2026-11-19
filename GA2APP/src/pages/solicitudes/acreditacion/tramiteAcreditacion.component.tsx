import React from 'react';
import { createStyles, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelectV2 from '../../../components/ch-select/CHSelectV2';

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

function ResumenAcreditacion({
	responseSolicitud,
}: {
	responseSolicitud: ResponseSeveranceModel | undefined;
}) {
	const classes = useStyles();
	return (
		<>
			<CHPaper title="Resumen acreditación">
				<Grid container>
					<Grid container>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								isReadOnly
								label="Tipo de solicitud"
								value="ACREDITACIÓN"
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHSelectV2
								label="Categoría"
								value={responseSolicitud?.apellidosCliente || ''}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<CHTextField
								label="Observaciones"
								value={
									responseSolicitud?.identificacionCliente || ''
								}
							/>
						</Grid>
					</Grid>
				</Grid>
			</CHPaper>
		</>
	);
}

export default ResumenAcreditacion;
