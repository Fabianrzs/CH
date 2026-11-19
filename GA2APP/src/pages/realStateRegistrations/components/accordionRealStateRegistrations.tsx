import { Accordion, AccordionDetails, AccordionSummary, createStyles, makeStyles, Typography, Button } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import CHPrivateContainer from "../../../components/ch-private-container/CHPrivateContainer";
import { AppState } from "../../../store/rootReducer";
import history from '../../../utils/history.router';
import BasicInformation from './basicInformation';
import Blocks from './blocks';
import PropietaryHistory from './propietaryHistory';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import ApiRealStateRegistrations from '../../../services/api-realstateregistrations.service';
import ChangeHistory from './ChangeHistory';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import ProcedureHistory from './procedureHistory';

const useStyles = makeStyles(() =>
    createStyles({
        firstAccordionTab: {
            color: '#ffffff',
            backgroundColor: '#1C3F8B',
            fontSize: '100',
            flexDirection: 'row-reverse',
            '&:hover': {
                backgroundColor: '#fec917',
            }
        },
        secondAccordionTab: {
            color: '#ffffff',
            backgroundColor: '#1C3F8B',
            flexDirection: 'row-reverse',
            '&:hover': {
                backgroundColor: '#fec917'
            }
        },
        thirdAccordionTab: {
            color: '#ffffff',
            backgroundColor: '#1C3F8B',
            flexDirection: 'row-reverse',
            '&:hover': {
                backgroundColor: '#fec917'
            }
        },
        forthAccordionTab: {
            color: '#ffffff',
            backgroundColor: '#1C3F8B',
            flexDirection: 'row-reverse',
            '&:hover': {
                backgroundColor: '#fec917'
            }
        },
        fiveAccordionTab: {
            color: '#ffffff',
            backgroundColor: '#1C3F8B',
            flexDirection: 'row-reverse',
            '&:hover': {
                backgroundColor: '#fec917',
            },
        },
        Typography: {
            fontFamily: 'OpenSans-Regular',
            fontSize: '22px',
        },
        processButton2: {
            backgroundColor: '#1C3F8B',
            color: '#FFFFFF',
            width: '12%',
            marginTop: '1%',
            marginLeft: '85%',
        },
    }),
);

function accodionRealStateRegistrations(props: linkMapsStateToProps): any {
    const classes = useStyles();
    const apiRealStateRegistrations = ApiRealStateRegistrations();
    const { realState } = props;
    const [nuevaData, setNuevaData] = useState<RealStateRegistrationsState | any>(realState.informacionBasica);

    useEffect(() => {
        cosultaTablactualizar()
    }, []);

    const cosultaTablactualizar = async () => {

        const request: any =
        {
            id: realState.informacionBasica.maiId
        }

        await apiRealStateRegistrations
            .GetBasicInformation(request.id)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setNuevaData(response?.data)
                    }
                }
            })
            .catch(e => {
                toast(`Error al consultar información básica: ${e.message}`, { type: toast.TYPE.ERROR });
            });
    }

    return (
        <CHPrivateContainer>
            <PageIndicators
                topTextNivel1='Matrículas'
                topTextNivel2='Matrículas Inmobiliarias' />

            <IconTitle
                subTitle='Inmobiliarias'
                title='Gestión De Matrículas'
                icon='fch-Inicio-0-1'
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
                    <BasicInformation consultar={cosultaTablactualizar} />
                </AccordionDetails>
            </Accordion>
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.secondAccordionTab}
                >
                    <Typography className={classes.Typography}>Historial de propietarios</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <PropietaryHistory dataActualizada={nuevaData} consultar={cosultaTablactualizar} />
                </AccordionDetails>
            </Accordion>
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#2ebbfe' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.thirdAccordionTab}
                >
                    <Typography className={classes.Typography}>Historial de cambios</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <ChangeHistory dataActualizada={nuevaData} consultar={cosultaTablactualizar} />
                </AccordionDetails>
            </Accordion>
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.forthAccordionTab}
                >
                    <Typography className={classes.Typography}>Bloqueos y desbloqueos</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Blocks consultar={cosultaTablactualizar} />
                </AccordionDetails>
            </Accordion>
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.fiveAccordionTab}
                >
                    <Typography className={classes.Typography}>Lista de Trámites</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <ProcedureHistory consultar={cosultaTablactualizar} />
                </AccordionDetails>
            </Accordion>
            <Button className={classes.processButton2}
                onClick={() => history.push({
                    pathname: '/Matriculas',
                    state: { nuevaData }
                })}>
                REGRESAR
            </Button>
        </CHPrivateContainer>
    )
}
interface linkMapsStateToProps {
    realState: RealStateRegistrationsState;
}
const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    realState: state.realStateRegistrations,

});

export default connect(mapStateToProps)(accodionRealStateRegistrations);