import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, Grid, makeStyles, Typography } from '@material-ui/core';
import { AppState } from '../../store/rootReducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import ParametrizacionSeguros from './components/componentsSeguros/ParametrizacionSeguros';
import ExcepSeguroHogar from './components/componentsSeguros/ExcepSeguroHogar';
import { startGetAllParametrizationSeguroRegion } from '../../actions/parametrization.action';
import { ParametrizacionState } from '../../reducers/parametrization.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        nota: {
            width: '100%',
            textAlign: 'left',
            color :'red',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1.2rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const Seguros = (props: props) => {
    const classes = useStyles();
    const { parametrizacionState, startGetAllParametrizationSeguroRegion } =
        props;

    useEffect(() => {
        startGetAllParametrizationSeguroRegion();
    }, []);

    return (
        <CHPrivateContainer>
            <CHPaper title="Parametrización de Seguros">
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Parámetros Generales
                    </Typography>
                </Grid>
                <Grid container>
                    <ParametrizacionSeguros />
                </Grid>
                <br />
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Factores para el cálculo de Seguro Hogar
                    </Typography>
                </Grid>
                <br />
                <Grid container>
                    <LoadingComponent loading={parametrizacionState?.loading} />
                    <ExcepSeguroHogar  parametrizacionState={parametrizacionState}/>
                </Grid>
            </CHPaper>

            {/* <Typography
               // className={classes.titleform}
               variant="h5"
               component="h6"
               className={classes.nota}
           >
               Nota: El factor a incluir debe estar en términos 
               porcentuales, se divide entre 12 y multiplica por el IVA.
               <br />
               Ejemplo:  0.08/12 * 1.19 = 0.0079
              
           </Typography> */}
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    parametrizacionState: ParametrizacionState;
}

const mapStateToProps = (state: AppState) => ({
    parametrizacionState: state.parametrization,
});

interface LinkMapDispatchToProps {
    startGetAllParametrizationSeguroRegion: () => void;
}

const mapDispatchToProps = {
    startGetAllParametrizationSeguroRegion,
};

type props = LinkMapDispatchToProps & LinkMapStateToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Seguros);
