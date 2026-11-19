import React, { useState } from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { toast } from 'react-toastify';
import { Controller } from 'react-hook-form';
import MaterialTable from 'material-table';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import {
    EmbargoConceptoDto,
    TipoEmbargoDto,
    TipoRetencionDto,
} from '../../types/embargo.model.type';
import apiEmbagos from '../../services/api-embargos.service';
import UtilService from '../../utils/util.service';
import { ModalAgregarEmbargosaConceptos } from './components/ModalAgregarEmbargoaConcepto';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const InformacionEmbargoConcepto = (props: props) => {
    const classes = useStyles();
    const apiEmbargo = apiEmbagos();
    const {
        control,
        getValues,
        tiposEmbargos,
        tiposRetencion,
        mostrarPorcentaje,
        handleSubmit,
        porcentaje,
        valor,
        errorPorcentaje,
        mensaje,
        openModal,
        cuentas,
        setCuentas,
        setOpenModal,
        setErrorPorcentaje,
        setMensaje,
        handleOnChangeMostrarPorcentaje,
        handleOnClickAdd,
        handleOnClickCancelar,
        handleOnChangeModo,
        handleChangeConceptoId,
    } = props;

    const columns: any = [
        {
            title: 'concepto',
            field: 'descripcionConcepto',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopLeftRadius: '12px',
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Saldo cuenta',
            field: 'conceptoSaldoCuenta',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) =>
                UtilService.formatCurrency(rowData.conceptoSaldoCuenta),
        },
    ];

    // const [cuentas, setCuentas] = useState([]);
    const [conceptos, setConceptos] = useState([]);
    const [mostrarTablaConceptos, setMostrarTablaConceptos] = useState(false);

    const handleOpenModal = (rowData: any) => {
        setOpenModal(true);
        handleOnChangeModo(false);
        handleChangeConceptoId(rowData.idConcepto);
    };
    const handleCloseModal = () => {
        setOpenModal(false);
        handleOnClickCancelar(false);
    };

    const obtenerCuentas = async (radicado: string) => {
        await apiEmbargo
            .getObtenerCuentasPorRadicadoWorkManager(radicado)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                       
                        setCuentas(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerConceptos = async (cuentaId: number) => {
        await apiEmbargo
            .getObtenerConceptoPorCuenta(cuentaId)
            .then(async response => {
                if (response) {
              
                    if (response.isSuccess && response.data.length > 0) {
                        setMostrarTablaConceptos(true);
                        setConceptos(response.data);
                    } else if (response.data.length === 0) {
                        setMostrarTablaConceptos(false);
                        toast('La cuenta no tiene conceptos.');
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleBuscarCuentas = async () => {
        // Activar select cuentas
        setCuentas([]);
        obtenerCuentas(getValues('radicadoWorkManager'));
    };

    const intercalar = (index: number) => {
        let color = '#f1f2f6';
        if (index % 2 === 0) {
            color = '#C4C4C4';
        }

        return {
            background: color,
            border: '1.5px solid #FFF',
        };
    };

    return (
        <>
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="radicadoWorkManager"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="No. Radicado Work Manager"
                                value={value}
                                onBlur={handleBuscarCuentas}
                                onChange={e => {
                                    handleBuscarCuentas();
                                    onChange(e);
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="cuentaId"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                        }}
                        render={({ field: { onChange, value } }) => (
                            <CHSelect
                                defaultValue=""
                                label="Cuentas"
                                value={value}
                                onChange={e => {
                                    obtenerConceptos(Number(e.target.value));
                                    // obtenerConceptos(Number(15));
                                    onChange(e);
                                }}
                                // Activar select cuentas
                                disabled={cuentas.length === 0}
                            >
                                {cuentas?.map((option: any) => (
                                    <MenuItem
                                        key={option.cuentaId}
                                        value={option.cuentaId}
                                    >
                                        {option.numeroCuenta}
                                    </MenuItem>
                                ))}
                            </CHSelect>
                        )}
                    />
                </Grid>
                {mostrarTablaConceptos && (
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={12}>
                            <MaterialTable
                                style={{
                                    borderTopLeftRadius: '12px',
                                    borderTopRightRadius: '12px',
                                    margin: '20px',
                                    width: '95%',
                                }}
                                title=""
                                data={conceptos}
                                columns={columns}
                                options={{
                                    toolbar: false,
                                    search: false,
                                    actionsColumnIndex: -1,
                                    selection: false,
                                    showEmptyDataSourceMessage: false,
                                    showTextRowsSelected: false,
                                    headerSelectionProps: {
                                        color: 'primary',
                                    },
                                    selectionProps: {
                                        color: 'primary',
                                    },
                                    headerStyle: {
                                        backgroundColor: '#FEC917',
                                        fontWeight: 'bold',
                                        color: '#676767',
                                        borderColor: '#FFF',
                                        border: '1.5px solid #FFF',
                                        borderTopRightRadius: '12px',
                                    },
                                    // pageSizeOptions: [10, 20, 30],
                                    filterCellStyle: {
                                        width: '50%',
                                    },
                                    rowStyle: (data, index) =>
                                        intercalar(index),
                                }}
                                actions={[
                                    {
                                        icon: 'add',
                                        tooltip: 'Agregar Concepto',
                                        onClick: (event, rowData: any) => {
                                          
                                            handleOpenModal(rowData);
                                        },
                                    },
                                ]}
                                localization={{
                                    header: {
                                        actions: 'Acciones',
                                    },
                                }}
                            />
                        </Grid>
                    </Grid>
                )}
            </Grid>
            <ModalAgregarEmbargosaConceptos
                open={openModal}
                handleClose={handleCloseModal}
                tiposEmbargos={tiposEmbargos}
                porcentaje={porcentaje}
                valor={valor}
                control={control}
                handleOnChangeMostrarPorcentaje={
                    handleOnChangeMostrarPorcentaje
                }
                mostrarPorcentaje={mostrarPorcentaje}
                setErrorPorcentaje={setErrorPorcentaje}
                setMensaje={setMensaje}
                errorPorcentaje={errorPorcentaje}
                mensaje={mensaje}
                tiposRetencion={tiposRetencion}
                handleSubmit={handleSubmit}
                handleOnClickAdd={handleOnClickAdd}
            />
        </>
    );
};

interface LinkMapDispatchToProps {
    tiposEmbargos: TipoEmbargoDto[];
    tiposRetencion: TipoRetencionDto[];
    control: any;
    getValues: any;
    mostrarPorcentaje: boolean;
    handleSubmit: any;
    porcentaje: string;
    valor: string;
    errorPorcentaje: boolean;
    mensaje: string;
    openModal: boolean;
    cuentas: any[];
    handleOnChangeMostrarPorcentaje: (value: boolean) => void;
    handleOnClickAdd: (data: EmbargoConceptoDto) => void;
    handleOnClickCancelar: (value: boolean) => void;
    handleOnChangeModo: (value: boolean) => void;
    handleChangeConceptoId: (value: number) => void;
    setErrorPorcentaje: (value: boolean) => void;
    setMensaje: (value: string) => void;
    setOpenModal: (value: boolean) => void;
    setCuentas: (value: any[]) => void;
}

type props = LinkMapDispatchToProps;

export default InformacionEmbargoConcepto;
