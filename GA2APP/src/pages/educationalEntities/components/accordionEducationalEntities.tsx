import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import {
	Accordion,
	AccordionSummary,
	Typography,
	AccordionDetails,
	createStyles,
	makeStyles,
	Grid,
	Button,
} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import history from '../../../utils/history.router';
import BasicInformationEntitiesComponent from './BasicInformationEntities.component';
import ProgramsComponent from './programs.component';
import CampusComponent from './campus.component';
import BlocksEntitiesComponent from './blocksEntities.component';
import AccountsComponent from './accounts.component';
import {
	startGetEducationalEntitiesDetail, startHistorialCambiosEntidades,
} from '../../../actions/educationalEntities.actions';
import FilesEntitiesComponent from './filesEntities.component';
import ProcedureHistoryEntity from './procedureHistoryEntity';
import ChangeHistory from './ChangeHistory';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles(() =>
	createStyles({
		firstAccordionTab: {
			color: '#ffffff',
			backgroundColor: '#1C3F8B',
			flexDirection: 'row-reverse',
			'&:hover': {
				backgroundColor: '#fec917',
			},
		},
		secondAccordionTab: {
			color: '#ffffff',
			backgroundColor: '#1C3F8B',
			flexDirection: 'row-reverse',
			'&:hover': {
				backgroundColor: '#fec917',
			},
		},
		thirdAccordionTab: {
			color: '#ffffff',
			backgroundColor: '#1C3F8B',
			flexDirection: 'row-reverse',
			'&:hover': {
				backgroundColor: '#fec917',
			},
		},
		fourthAccordionTab: {
			color: '#ffffff',
			backgroundColor: '#1C3F8B',
			flexDirection: 'row-reverse',
			'&:hover': {
				backgroundColor: '#fec917',
			},
		},
		fifthAccordionTab: {
			color: '#ffffff',
			backgroundColor: '#1C3F8B',
			flexDirection: 'row-reverse',
			'&:hover': {
				backgroundColor: '#fec917',
			},
		},
		sixthAccordionTab: {
			color: '#ffffff',
			backgroundColor: '#1C3F8B',
			flexDirection: 'row-reverse',
			'&:hover': {
				backgroundColor: '#fec917',
			},
		},
		button3: {
			width: '200px',
			marginLeft: '80rem',
			background: '#1C3F8B',
			color: '#FFFFFF',
			textAlign: 'center',
			borderRadius: '5%',
			marginTop: '10px',
		},
		Typography: {
			fontFamily: 'OpenSans-Regular',
			fontSize: '22px',
		},
	}),
);

function AccordionEducationalEntities(): JSX.Element {

	const classes = useStyles();
	const { idEntidad } = useParams<any>();
	const user = AuthService.getUserInfo();
	const dispatch = useDispatch();

	useEffect(() => {
		dispatch(startGetEducationalEntitiesDetail(idEntidad));
		dispatch(startHistorialCambiosEntidades(idEntidad));
	}, [idEntidad]);

	return (
		<CHPrivateContainer>
			<CHPageTitleV2
				topText="Gestión de afiliados"
				topText2="Gestión de entidades"
				title1="Gestión de entidades"
				title2="educativas"
				icono="school"
			/>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.firstAccordionTab}
				>
					<Typography className={classes.Typography}>Información básica</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<BasicInformationEntitiesComponent />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.secondAccordionTab}
				>
					<Typography className={classes.Typography}>Programas</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<ProgramsComponent idEntidad={idEntidad} usuario={user} />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.thirdAccordionTab}
				>
					<Typography className={classes.Typography}>Sedes</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<CampusComponent idEntidad={idEntidad} usuario={user} />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.fourthAccordionTab}
				>
					<Typography className={classes.Typography}>Bloqueos/Desbloqueos</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<BlocksEntitiesComponent idEntidad={idEntidad} />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.fifthAccordionTab}
				>
					<Typography className={classes.Typography}>Fideicomiso</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<AccountsComponent idEntidad={idEntidad} usuario={user} />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.sixthAccordionTab}
				>
					<Typography className={classes.Typography}>Anotaciones</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<FilesEntitiesComponent idEntidad={idEntidad} />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.sixthAccordionTab}
				>
					<Typography className={classes.Typography}>Lista de tramites</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<ProcedureHistoryEntity idEntity={idEntidad} />
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
					aria-controls="panel1a-content"
					id="panel1a-header"
					className={classes.sixthAccordionTab}
				>
					<Typography className={classes.Typography}>Historial de cambios</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<ChangeHistory />
				</AccordionDetails>
			</Accordion>
			<Grid item md={5}>
				<Button
					className={classes.button3}
					onClick={() => history.push('/EntidadesEducativas')}
				>
					REGRESAR
				</Button>
			</Grid>
		</CHPrivateContainer>
	);
}

export default AccordionEducationalEntities;