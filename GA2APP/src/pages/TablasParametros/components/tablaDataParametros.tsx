
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import {
    Button,
    createStyles,
    Grid,
    IconButton,
    makeStyles,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { Edit } from '@material-ui/icons';
import 'date-fns';
import { connect } from 'react-redux';
import { useHistory } from 'react-router-dom';
import '../../../estiloprincipal.scss';
import {
    startInsertData,
} from '../../../actions/tablaParametros.action';
import { AppState } from '../../../store/rootReducer';
import { DataEditForm, TABLA_PARAMETRO } from '../constants/tablaParametroform.model.type';
import { LoginState } from '../../../reducers/login.reducer';
import { DataData } from '../../../reducers/tablaParametros.reducer';
import { ModalAgregar } from './modalAgregar';
import { ModalEditar } from './modalEditar';
import { TablaTitleParametroDto } from '../../../types/tablaParametro.model.type';
import { ModalFuerza } from './modalFuerza';
import ModalUnidadesEjecutoras from './modalUnidadesEjecutoras';

const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        processButton: {
            backgroundColor: '#42b7ef',
            color: '#fff',
            width: '14%',
            marginLeft: '5%',
        },
        tableHeader: {
            backgroundColor: '#FEC917',
            borderBottom: '1px solid #e0e0e0',
            textAlign: 'center',
        },
        alternateRow: {
            backgroundColor: '#D3D3D3',
        },
        textCell: {
            textAlign: 'center',
        },
        checkBox: {
            backgroundColor: '#0075ff',
            accentColor: '#1c3f8b',
            width: '18px',
            height: '18px',
            cursor: 'pointer',
        },
    }),
);

const DatosTable = (props: Props) => {

    const {
        dataData,
        loginState,
        load,
        startInsertData,
        data,
        dataTitle,
        isDataLoaded,
        descripcionSeleccionada,
        valueTablaParametroSeleccionado,
    } = props;

    const classes = useStyles();
    const history = useHistory();
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalAddFuerza, setOpenModalAddFuerza] = useState(false);
    const [openModalAddUEJ, setOpenModalAddUEJ] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);
    const [formState, setFormState] = useState(DataEditForm);
    const [selectedRows, setSelectedRows] = useState<any>([]);
    const [selectAll, setSelectAll] = useState(false);

    const setFormData = () => {
        setFormState(dataData.data);
    };

    useEffect(() => {
        setFormData();
    }, [0]);

    const handleOpenModalAgregar = () => {
        if (descripcionSeleccionada === 'FUERZAS') {
            setOpenModalAddFuerza(true);
        } else if (descripcionSeleccionada === 'UNIDADES EJECUTORAS') {
            setOpenModalAddUEJ(true);
        } else {
            setOpenModalAgregar(true);
        }
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleCloseModalAddFuerza = () => {
        setOpenModalAddFuerza(false);
    };

    const handleCloseModalAddUEJ = () => {
        setOpenModalAddUEJ(false);
    };

    const handleOpenModalEditar = (rowData: any) => {
        if(valueTablaParametroSeleccionado === TABLA_PARAMETRO.UNIDADES_EJECUTORAS){
            history.push('/InformacionUnidadEjecutora', rowData);
        }else{
            setOpenModalEditar(true);
        }
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);

    };

    const handleOnClickInsert = () => {
        insertState();
    };

    const handleOnClickEditar = () => {

    };

    const setNewData = () => {
        setTimeout(() => {
            load();
        }, 500);
    };

    const insertState = () => {

        handleCloseModalEditar();
        handleCloseModalAgregar();
        startInsertData(formState, loginState.claims);
        setNewData();
    };

    const handleOnChange = ({ target }: any) => {

    };

    if (!isDataLoaded) {
        return null;
    }

    if (data.length === 0) {
        return <div>No se encontraron resultados</div>;
    }

    const handleSelectAll = () => {
        if (selectAll) {
            setSelectedRows([]);
        } else {
            const allRowIndices: any = data.map((_, index) => index);
            setSelectedRows(allRowIndices);
        }
        setSelectAll(!selectAll);
    };

    const handleRowSelect = (index: any) => {
        if (selectedRows.includes(index)) {
            setSelectedRows(selectedRows.filter((row: any) => row !== index));
        } else {
            setSelectedRows([...selectedRows, index]);
        }
    };

    const transformColumnName = (columnName: any) => {
        switch (columnName) {
            case 'ENT_ID':
                return 'CÓDIGO ID';
            case 'ENT_CODIGO_BANCO':
                return 'CÓDIGO DEL BANCO';
            case 'ENT_ESTADO':
                return 'ESTADO';
            case 'ENT_CREATEDOPOR':
                return 'CREADO POR';
            case 'ENT_FECHACREACION':
                return 'FECHA DE CREACIÓN';
            case 'ENT_MODIFICADOPOR':
                return 'MODIFICADO POR';
            case 'ENT_FECHAMODIFICACION':
                return 'FECHA DE MODIFICACIÓN';
            default:
                return columnName.replace(/^ENT_/, '').replace(/_/g, ' ');
        }
    };

    const originalColumnCount = dataTitle.length + 1;

    const filteredDataTitle = dataTitle.filter((row) =>
        row.nombreColumna !== 'ENT_FECHACREACION'
    );

    const colSpanValue = originalColumnCount - filteredDataTitle.length;

    return (
        <>

            <Grid className={classes.column} item xs={12}>
                <Grid className={classes.column} item xs={1}>
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        onClick={handleOpenModalAgregar}
                    >
                        Agregar
                    </Button>
                </Grid>
                <TableContainer>
                    <Table>
                        <TableHead>
                            <TableRow>
                                {valueTablaParametroSeleccionado === TABLA_PARAMETRO.FUERZAS && (
                                    <TableCell className={classes.tableHeader}>
                                        <input
                                            type="checkbox"
                                            checked={selectAll}
                                            onChange={handleSelectAll}
                                            className={classes.checkBox}
                                        />
                                    </TableCell>
                                )}
                                {/* {filteredDataTitle.map((row, index) => (
                                    <TableCell key={index} className={classes.tableHeader}>
                                        {transformColumnName(row.nombreColumna)}
                                    </TableCell>
                                ))} */}
                                {filteredDataTitle
                                    .filter((row) =>
                                        valueTablaParametroSeleccionado !== TABLA_PARAMETRO.BANCOS || row.nombreColumna !== 'ENT_ID'
                                    )
                                    .map((row, index) => (
                                        <TableCell key={index} className={classes.tableHeader}>
                                            {transformColumnName(row.nombreColumna)}
                                        </TableCell>
                                    ))}
                                <TableCell className={classes.tableHeader} colSpan={colSpanValue}>Acciones</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {data.map((rowData, indexData: any) => (
                                <TableRow key={indexData} className={indexData % 2 === 0 ? classes.alternateRow : ''}>
                                    {valueTablaParametroSeleccionado === TABLA_PARAMETRO.FUERZAS && (
                                        <TableCell className={classes.textCell}>
                                            <input
                                                type="checkbox"
                                                checked={selectedRows.includes(indexData)}
                                                onChange={() => handleRowSelect(indexData)}
                                                className={classes.checkBox}
                                            />
                                        </TableCell>
                                    )}
                                    {dataTitle
                                        .filter((rowDataTitle) =>
                                            valueTablaParametroSeleccionado !== TABLA_PARAMETRO.BANCOS || rowDataTitle.nombreColumna !== 'ENT_ID'
                                        )
                                        .map((rowDataTitle, indexDataTitle) => (
                                            <TableCell key={indexDataTitle} className={classes.textCell}>
                                                {rowData[rowDataTitle.nombreColumna]}
                                            </TableCell>
                                        ))}
                                    <TableCell className={classes.textCell}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            onClick={() => handleOpenModalEditar(rowData)}
                                        >
                                            <Edit />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
                <br/>
                <br/>
                {valueTablaParametroSeleccionado === TABLA_PARAMETRO.FUERZAS && (

                    <Grid container spacing={3} justify="flex-end">
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                            // onClick={() => handleOnClickAgregar}

                            >
                                Adicionar grado
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                // onClick={handleClose}
                                style={{ marginLeft: "10px" }}
                            >
                                Eliminar grado
                            </Button>
                        </Grid>

                    </Grid>
                )}

                <ModalAgregar
                    open={openModalAgregar}
                    handleClose={handleCloseModalAgregar}
                    handleOnClickAgregar={handleOnClickInsert}
                    handleOnChange={handleOnChange}
                    data={dataTitle}
                />
                <ModalEditar
                    open={openModalEditar}
                    handleClose={handleCloseModalEditar}
                    handleOnClickAgregar={handleOnClickEditar}
                    handleOnChange={handleOnChange}
                    data={dataTitle}
                    dataEdit={formState}
                    valueTablaParametroSeleccionado={valueTablaParametroSeleccionado}
                />
                <ModalFuerza
                    open={openModalAddFuerza}
                    handleClose={handleCloseModalAddFuerza}
                />
                <ModalUnidadesEjecutoras
                    open={openModalAddUEJ}
                    handleClose={handleCloseModalAddUEJ}
                    catalogState={undefined}
                />
            </Grid>
        </>
    );
}

interface LinkMapStateToProps {
    loginState: LoginState;
    dataData: DataData;
    data: any[];
    dataTitle: TablaTitleParametroDto[];
    isDataLoaded: boolean;
    descripcionSeleccionada: any;
    valueTablaParametroSeleccionado: string;
}

const mapStateToProps = (data: AppState) => ({
    loginData: data.login,
    dataData: data.data,
    catalogState: data.catalog,
});

interface LinkMapDispatchToProps {
    startInsertData: (State: any, props: string) => void;
    load: () => void;
}

const mapDispatchToProps = {
    startInsertData,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(DatosTable);