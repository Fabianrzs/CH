import React, { useState } from 'react';
import CloudUploadIcon from '@material-ui/icons/CloudUpload';
import {
    Button,
    Card,
    CardContent,
    Container,
    Dialog,
    makeStyles,
    Typography,
} from '@material-ui/core';
import MaterialTable from 'material-table';

import { connect } from 'react-redux';
import ModalEditarParametro from './modal-editar-parametro.component';
import ModalInsertarParametro from './modal-insertar-parametro.component';
import { ParametroTransaccion } from '../constants/form.model.type';
import { AppState } from '../../../store/rootReducer';
// import { ParametrizacionState } from '../../../reducers/parametrization.reducer';

const columnas = [
    {
        title: 'Concepto',
        field: 'paraM_TRANS_CONCEPTO',
    },
    {
        title: 'Codigo',
        field: 'paraM_TRANS_CODIGO',
    },
    {
        title: 'Proceso',
        field: 'paraM_TRANS_PROCESO',
    },
];

const useStyles = makeStyles(theme => ({
    modal: {
        position: 'absolute',
        width: 400,
        backgroundColor: theme.palette.background.paper,
        border: '2px solid #000',
        boxShadow: theme.shadows[5],
        padding: theme.spacing(2, 4, 3),
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
    },
    iconos: {
        cursor: 'pointer',
    },
    inputMaterial: {
        width: '100%',
    },
}));

export const Concepts = () => {
    const Data: [] = [];
    const [conceptoState, setConceptoState] = useState(ParametroTransaccion);
    const [openStateEditar, setOpenStateEditar] = useState(false);
    const [openStateInsertar, setOpenStateInsertar] = useState(false);
    useStyles();


   
    const handleOpenModalInsertar = () => {
        setOpenStateInsertar(true);
    };

    const handleCloseModalInsertar = () => {
        setOpenStateInsertar(false);
    };

    const handleCloseModalEditar = () => {
        setOpenStateEditar(false);
    };

    return (
        <Container maxWidth="lg" className="contenedores">
            <br />
            <br />
            <Card>
                <CardContent>
                    <Typography variant="h6" align="center" className="titulo1">
                        Parametrización
                    </Typography>
                    <Typography variant="h6" align="center" className="titulo2">
                        conceptos
                    </Typography>
                    <br />
                    <br />
                    <Button
                        variant="contained"
                        color="primary"
                        startIcon={<CloudUploadIcon />}
                        onClick={handleOpenModalInsertar}
                    >
                        Insertar Concepto
                    </Button>

                    <Dialog
                        open={openStateInsertar}
                        onClose={handleCloseModalInsertar}
                        aria-labelledby="simple-dialog-title"
                    >
                        <ModalInsertarParametro
                            handleCloseModalInsertar={handleCloseModalInsertar}
                        />
                    </Dialog>

                    <Dialog
                        open={openStateEditar}
                        onClose={handleCloseModalEditar}
                        aria-labelledby="simple-dialog-title"
                    >
                        <ModalEditarParametro
                            handleCloseModalEditar={handleCloseModalEditar}
                            conceptoState={conceptoState}
                            setConceptoState={setConceptoState}
                        />
                    </Dialog>

                    <br />
                    <br />

                    <br />
                    <br />

                    <MaterialTable
                        columns={columnas}
                        data={Data != null ? Data : []}
                        title="Conceptos"
                        actions={[
                            {
                                icon: 'edit',
                                tooltip: 'Editar Concepto',
                                onClick: (event, rowData: any) => {
                                    setOpenStateEditar(true);
                                    setConceptoState(rowData);
                                },
                            },
                        ]}
                        options={{
                            actionsColumnIndex: -1,
                        }}
                        localization={{
                            header: {
                                actions: 'Acciones',
                            },
                        }}
                    />
                </CardContent>
            </Card>
        </Container>
    );
};
const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    conceptoState: state.parametrization,
});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(Concepts);
