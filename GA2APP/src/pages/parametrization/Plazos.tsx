import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
    Typography
} from '@material-ui/core';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../components/ch-paper/CHPaper';
import ParametrizacionPlazos from './components/componentsPlazos/ParametrizacionPlazos';
import ExcepPlazosTipoActor from './components/componentsPlazos/ExcepPlazosTipoActor';
import {
    startGetAllParametrizationPlazosGeneral,
    startPostAllParametrizationPlazosTipoActor,
    startPostAllParametrizationPlazosUniej
} from '../../actions/parametrization.action';
import { startGetCreditType } from '../../actions/catalog.action';
import { ParametrizacionState } from '../../reducers/parametrization.reducer';
import { AppState } from '../../store/rootReducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import CHSelect from '../../components/ch-select/CHSelect';
import ExcepPlazosUnidadEjecutora from './components/componentsPlazos/ExcepPlazosUnidadEjecutora';
import { CatalogState } from '../../reducers/catalog.reducer';
import { CreditTypeModel } from '../../models/credit-type.model';

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
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const Plazos = (props: props) => {

    const classes = useStyles();

    const { parametrizacionState,
        catalogState,
        startGetAllParametrizationPlazosGeneral,
        startGetCreditType,
        startPostAllParametrizationPlazosTipoActor,
        startPostAllParametrizationPlazosUniej } = props;

    const [idTipoCreditoTipoActor, setIdTipoCreditoTipoActor] = useState(1);
    const [idTipoCreditoUnidadEjecutora, setIdTipoCreditoUnidadEjecutora] = useState(1);

    const load = () => {
        startGetAllParametrizationPlazosGeneral();
    }

    const loadTipoAfiliado = () => {
        startPostAllParametrizationPlazosTipoActor(idTipoCreditoTipoActor);
    }

    const loadUnidadEjecutora = () => {
        startPostAllParametrizationPlazosUniej(idTipoCreditoUnidadEjecutora);
    }

    useEffect(() => {
        load();
        loadTipoAfiliado();
        loadUnidadEjecutora();
        startGetCreditType();
    }, []);

    useEffect(() => {
        loadTipoAfiliado();
    }, [idTipoCreditoTipoActor]);

    useEffect(() => {
        loadUnidadEjecutora();
    }, [idTipoCreditoUnidadEjecutora]);

    const handleOnChangePlazoTipoActor = (target: any) => {
        setIdTipoCreditoTipoActor(target.value);
    }

    const handleOnChangePlazoUnidadEjecutora = (target: any) => {
        setIdTipoCreditoUnidadEjecutora(target.value);
    }

    return (
        <CHPrivateContainer>
            <CHPaper title="Parametrización de Plazos">
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Parámetros Generales
                    </Typography>
                </Grid>
                <br />
                <Grid container>
                    <LoadingComponent loading={parametrizacionState?.loading} />
                    <ParametrizacionPlazos load={load} />
                </Grid>
                <br />
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Excepciones por tipo afiliado
                    </Typography>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <CHSelect
                            label="Línea / Tipo Crédito"
                            name="idTipoCreditoTipoActor"
                            value={idTipoCreditoTipoActor}
                            onChange={e => handleOnChangePlazoTipoActor(e.target)}
                        >
                            {
                                catalogState.creditTypes?.map(
                                    (option: CreditTypeModel) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.description}
                                        </MenuItem>
                                    ),
                                )}
                        </CHSelect>
                    </Grid>
                </Grid>
                <Grid container>
                    <LoadingComponent loading={parametrizacionState?.loading} />
                    <ExcepPlazosTipoActor loadTipoAfiliado={loadTipoAfiliado} tipoCreditoSelected={idTipoCreditoTipoActor} />
                </Grid>
                <br />
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Excepciones por unidad ejecutora
                    </Typography>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <CHSelect
                            label="Línea / Tipo Crédito"
                            name="idTipoCreditoUnidadEjecutora"
                            value={idTipoCreditoUnidadEjecutora}
                            onChange={e => handleOnChangePlazoUnidadEjecutora(e.target)}
                        >
                            {
                                catalogState.creditTypes?.map(
                                    (option: CreditTypeModel) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.description}
                                        </MenuItem>
                                    ),
                                )}
                        </CHSelect>
                    </Grid>
                </Grid>
                <Grid container>
                    <LoadingComponent loading={parametrizacionState?.loading} />
                    <ExcepPlazosUnidadEjecutora loadUnidadEjecutora={loadUnidadEjecutora} tipoCreditoSelected={idTipoCreditoUnidadEjecutora} />
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    );
}
interface LinkMapStateToProps {
    parametrizacionState: ParametrizacionState;
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    parametrizacionState: state.parametrization,
    catalogState: state.catalog
});

interface LinkMapDispatchToProps {
    startGetAllParametrizationPlazosGeneral: () => void;
    startGetCreditType: () => void;
    startPostAllParametrizationPlazosTipoActor: (idTipoCredito: number) => void;
    startPostAllParametrizationPlazosUniej: (idTipoCredito: number) => void;
}

const mapDispatchToProps = {
    startGetAllParametrizationPlazosGeneral,
    startGetCreditType,
    startPostAllParametrizationPlazosTipoActor,
    startPostAllParametrizationPlazosUniej
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Plazos);
