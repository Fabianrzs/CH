import { Container, Grid } from '@material-ui/core';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';

import { startGetAllExecutingUnits, 
         startGetAllTypeFiles,
         startGetAllStateFiles
        } from '../../actions/catalog.action';
import {
    startPostArchivoNomina,
    startObtenerArchivosCargados,
    startConsultarArchivosCargados
} from '../../actions/cargue-nomina.action';
import DataTableNomina from './components/DataTableNomina';
import { LoadingComponent } from '../../components/loading/loading.component';
import { CargueNominaState } from '../../reducers/cargue-nomina.reducer';
import { LoginState } from '../../reducers/login.reducer';
import FileUploadComponent from './components/file-upload.component';

const CargueNomina = (props: Props) => {
    const {
        loginState,
        cargueNominaState,
        startObtenerArchivosCargados,
        startPostArchivoNomina,
        startGetAllExecutingUnits,
        startGetAllTypeFiles,
        startGetAllStateFiles,
        startConsultarArchivosCargados
    } = props;
    const esdId = 1;
    const loadData = () => {
        startGetAllExecutingUnits(loginState.claims);
        startGetAllTypeFiles(loginState.claims);
        startGetAllStateFiles(loginState.claims);
        startObtenerArchivosCargados(esdId);
        // startConsultarArchivosCargados(esdId);
        
    };

    useEffect(() => {
        loadData();
    }, []);

    
    return (
        <Container>
            <LoadingComponent loading={cargueNominaState.loading} />
            <Grid container spacing={2}>
            <Grid item xs={12}>
  {/* <FileUploadComponent key="FileUpload" /> */}
</Grid>
                <Grid item xs={12}>
                    <DataTableNomina />
                </Grid>
            </Grid>
        </Container>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    cargueNominaState: CargueNominaState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    cargueNominaState: state.cargueNomina,
});

interface LinkMapDispatchProps {
    startGetAllExecutingUnits: (props: string) => void;
    startGetAllTypeFiles: (props: string) => void;
    startGetAllStateFiles: (props: string) => void;
    startPostArchivoNomina: (formData: FormData) => void;
    startObtenerArchivosCargados: (esdId: number) => void;
    startConsultarArchivosCargados: (esdId: number) => void;
}

const mapDispatchToProps = {
    startGetAllExecutingUnits,
    startGetAllTypeFiles,
    startGetAllStateFiles,
    startObtenerArchivosCargados,
    startPostArchivoNomina,
    startConsultarArchivosCargados,
};

type Props = LinkMapDispatchProps & LinkMapStateToProps;

export default connect(mapStateToProps, mapDispatchToProps)(CargueNomina);
