import React, { useEffect, useState } from 'react';
import { createStyles, makeStyles, Typography } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import TableEmbargoConcepto from './TableEmbargoConceptos';
import {
    startGetTiposEmbargo,
    startGetTiposRetencion,
    startGetAllEmbargosCuentaConcepto,
    startGetAllConceptosEmbargosById,
} from '../../actions/embargos.action';
import { EmbargosState } from '../../reducers/embargos.reducer';
import { AppState } from '../../store/rootReducer';
import { EmbargoConceptoDto } from '../../types/embargo.model.type';
import { embargoConceptoForm } from '../embargos/constants/embargosform.model.type';
import InformacionEmbargoConcepto from './InformacionEmbargoConcepto';
import apiEmbargos from '../../services/api-embargos.service';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';
import DetalleEmbargosConcepto from './DetalleEmbargosConcepto';

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

const EmbargoConcepto = (props: props) => {
    const classes = useStyles();
    const api = apiEmbargos();

    const {
        embargoState,
        startGetTiposEmbargo,
        startGetTiposRetencion,
        startGetAllEmbargosCuentaConcepto,
        startGetAllConceptosEmbargosById,
    } = props;

    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const valor = '95ca1858-b89d-4631-9893-22932c925184';
    const [modoActualizar, setModoActualizar] = useState(false);
    const [mostrarPorcentaje, setMostrarPorcentaje] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);
    const [openModal, setOpenModal] = useState(false);
    const [conceptoId, setConceptoId] = useState(0);
    const [errorPorcentaje, setErrorPorcentaje] = useState(false);
    const [mensaje, setMensaje] = useState('');
    const [indicador, setIndicador] = useState(false);
    const [idEmbargo, setIdEmbargo] = useState('');
    const [cuentas, setCuentas] = useState<any[]>([]);

    const handleOpenModalEditar = (value: any) => {
        setOpenModalEditar(true);
        handleOnChangeEdit(value);
    };

    const handleCloseModalEditar = async () => {
        setOpenModalEditar(false);
        // si es modo actualizar
        await handleOnChangeModo(false);
        await handleOnChangeMostrarPorcentaje(false);
        reset();
    };

    const { handleSubmit, control, setValue, reset, getValues } =
        useForm<EmbargoConceptoDto>({
            defaultValues: embargoConceptoForm,
        });

    const handleLoadTiposEmbargo = async () => {
        await startGetTiposEmbargo();
    };

    const handleLoadTiposRetencion = async () => {
        await startGetTiposRetencion();
    };

    const handleChangeConceptoId = (value: number) => setConceptoId(value);

    useEffect(() => {
        handleLoadTiposEmbargo();
        handleLoadTiposRetencion();
        handleLoad();
    }, []);

    useEffect(() => {
        if (idEmbargo !== '') {
            startGetAllConceptosEmbargosById(idEmbargo);
        }
    }, [embargoState.conceptos.conceptos, idEmbargo]);

    const handleLoad = async () => {
        await startGetAllEmbargosCuentaConcepto();
    };

    const handleOnChangeModo = (value: boolean) => setModoActualizar(value);

    const handleOnChangeMostrarPorcentaje = (value: boolean) =>
        setMostrarPorcentaje(value);

    const handleOnChangeEdit = async (value: any) => {
        setValue('id', value.id);
        if (value.tipoembargo === 'Porcentaje') {
            handleOnChangeMostrarPorcentaje(true);
        } else {
            handleOnChangeMostrarPorcentaje(false);
        }
        setValue('valor', value.valor);
        setValue('fechaInicio', value.fechaInicio);
        setValue('fechaFin', value.fechaFin);
        setValue('indicadorCesantias', value.indicadorCesantias);
        setValue('tipoEmbargoId', value.tipoEmbargoId);
        setValue('tipoRetencionId', value.tipoRetencionId);
        setValue('radicadoWorkManager', value.radicadoWorkManager);
        setValue('numeroCuenta', value.numeroCuenta);
    };

    const handleOnClickCancelar = async (value: boolean) => {
        await handleOnChangeModo(value);
        await handleOnChangeMostrarPorcentaje(false);
        reset();
    };

    const insertarEmbargoConcepto = (
        embargoConcepto: EmbargoConceptoDto,
        conceptoId: number,
    ) => api.postInsertEmbargoCuentaConcepto(embargoConcepto, conceptoId);

    const actualizarEmbargoConcepto = (embargoConcepto: EmbargoConceptoDto) =>
        api.postUpdateEmbargoCuentaConcepto(embargoConcepto);

    const handleOnClickValidation = async (data: EmbargoConceptoDto) => {
       
        if (data.valor.toString().includes('$') && data.valor !== 0) {
            data.valor = UtilService.cleanCurrencyValue(data.valor);
            handleOnClick(data);
        } else if (
            data.tipoEmbargoId === '95ca1858-b89d-4631-9893-22932c925184' &&
            data.valor !== 0
        ) {
            handleOnClick(data);
        } else if (data.valor) {
            data.valor = Number(data.valor.toString().replace('%', ''));
            if (Number(data.valor) <= 100) {
                handleOnClick(data);
            } else {
                swal({
                    text: 'El valor no puede ser mayor que 100',
                    icon: 'warning',
                });
            }
        }
    };

    const handleOnClick = async (data: EmbargoConceptoDto) => {
        if (data.tipoEmbargoId === '') {
            swal({
                text: 'El campo tipo de embargo es requerido',
                icon: 'warning',
            });
        } else if (data.valor === 0) {
            swal({
                text: 'El valor debe ser mayor a cero',
                icon: 'warning',
            });
        } else if (data.tipoRetencionId === '') {
            swal({
                text: 'El campo tipo retención es requerido',
                icon: 'warning',
            });
        } else if (data.fechaInicio > data.fechaFin) {
            swal({
                text: 'La fecha inicio no puede ser mayor que la fecha fin',
                icon: 'warning',
            });
        } else if (data.fechaFin < data.fechaInicio) {
            swal({
                text: 'La fecha inicio no puede ser mayor que la fecha fin',
                icon: 'warning',
            });
        } else if (!modoActualizar) {
            const respuesta = await insertarEmbargoConcepto(data, conceptoId);
            if (respuesta?.isSuccess === false) {
                swal({
                    text: respuesta.returnMessage,
                    icon: 'warning',
                });
            } else {
                reset();
                handleLoad();
                setOpenModal(false);
                await handleOnChangeMostrarPorcentaje(false);
                await NotificationService.genericAlert(
                    'Guardado',
                    'Se ha guardado correctamente la información',
                );
            }
        } else {
            const respuesta = await actualizarEmbargoConcepto(data);
            if (respuesta?.isSuccess === false) {
                swal({
                    text: respuesta.returnMessage,
                    icon: 'warning',
                });
            } else {
                await handleOnChangeModo(false);
                await handleOnChangeMostrarPorcentaje(false);
                reset();
                handleLoad();
                handleCloseModalEditar();
                await NotificationService.genericAlert(
                    'Actualizado con éxito',
                    'Se realizó con éxito la actualización.',
                );
            }
        }
    };

    const onClickDetalleEmbargo = (value: string) => {
        startGetAllConceptosEmbargosById(value);
        setIdEmbargo(value);
        if (embargoState.conceptos.conceptos) {
            setIndicador(true);
        } else {
            setIndicador(false);
        }
    };

    const onItemGoBack = () => setIndicador(false);

    if (indicador) {
        return (
            <CHPrivateContainer>
                <DetalleEmbargosConcepto
                    tiposEmbargos={embargoState.tiposEmbargo}
                    tiposRetencion={embargoState.tiposRetencion}
                    control={control}
                    openModalEditar={openModalEditar}
                    porcentaje={porcentaje}
                    valor={valor}
                    conceptosEditar={embargoState.conceptos.conceptos}
                    mostrarPorcentaje={mostrarPorcentaje}
                    handleOnChangeMostrarPorcentaje={
                        handleOnChangeMostrarPorcentaje
                    }
                    errorPorcentaje={errorPorcentaje}
                    setErrorPorcentaje={setErrorPorcentaje}
                    mensaje={mensaje}
                    setMensaje={setMensaje}
                    handleCloseModalEditar={handleCloseModalEditar}
                    handleOpenModalEditar={handleOpenModalEditar}
                    handleOnChangeModo={handleOnChangeModo}
                    handleOnClickValidation={handleOnClickValidation}
                    handleSubmit={handleSubmit}
                    onItemGoBack={onItemGoBack}
                />
            </CHPrivateContainer>
        );
    }

    return (
        <CHPrivateContainer>
            <CHPaper title="Conceptos a embargar">
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Información Embargo a conceptos
                </Typography>
                <InformacionEmbargoConcepto
                    porcentaje={porcentaje}
                    valor={valor}
                    tiposEmbargos={embargoState.tiposEmbargo}
                    tiposRetencion={embargoState.tiposRetencion}
                    control={control}
                    getValues={getValues}
                    mostrarPorcentaje={mostrarPorcentaje}
                    errorPorcentaje={errorPorcentaje}
                    handleOnChangeMostrarPorcentaje={
                        handleOnChangeMostrarPorcentaje
                    }
                    handleSubmit={handleSubmit}
                    handleOnClickAdd={handleOnClickValidation}
                    handleOnClickCancelar={handleOnClickCancelar}
                    handleOnChangeModo={handleOnChangeModo}
                    handleChangeConceptoId={handleChangeConceptoId}
                    setErrorPorcentaje={setErrorPorcentaje}
                    setMensaje={setMensaje}
                    mensaje={mensaje}
                    openModal={openModal}
                    setOpenModal={setOpenModal}
                    cuentas={cuentas}
                    setCuentas={setCuentas}
                />
                <br />
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista Embargo a conceptos
                </Typography>
                <br />
                <TableEmbargoConcepto
                    onClickDetalleEmbargo={onClickDetalleEmbargo}
                    load={handleLoad}
                />
            </CHPaper>
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    embargoState: EmbargosState;
}

const mapStateToProps = (state: AppState) => ({
    embargoState: state.embargo,
});
interface LinkMapDispatchToProps {
    startGetTiposEmbargo: () => void;
    startGetTiposRetencion: () => void;
    startGetAllEmbargosCuentaConcepto: () => void;
    startGetAllConceptosEmbargosById: (idEmbargo: string) => void;
}

const mapDispatchToProps = {
    startGetTiposEmbargo,
    startGetTiposRetencion,
    startGetAllEmbargosCuentaConcepto,
    startGetAllConceptosEmbargosById,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(EmbargoConcepto);
