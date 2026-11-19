import React, { useEffect, useState } from 'react';
import { toast } from "react-toastify";
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles, Typography, Button, } from '@material-ui/core';
import swal from 'sweetalert';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import { PopupEstadoCliente } from './PopupEstadoCliente';
import ApiClients from '../../../services/api-clients.service';
import { PopupRazonCausal } from './PopupRazonCausal';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import history from '../../../utils/history.router';
import { LoginState } from '../../../reducers/login.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center'
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        titleCampo: {
            fontSize: '10px',

        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
            borderRadius: '10%',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
        centeredHeader: {
            textAlign: 'center',
        },
    }),
);

function Estados({
    catalogState,
    clientModel,
    loginState,
    setClientModel
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
    loginState: LoginState;
    setClientModel: React.Dispatch<React.SetStateAction<ClienteDto>> | undefined;
}) {



    const api = ApiClients();
    const [estados, setEstados] = useState<boolean>(false);
    const [modalOpen, setModalOpen] = useState(false);
    const [modalRazonOpen, setModalRazonOpen] = useState(false);
    const [idModalRazon, setIdModalRazon] = useState<number>();
    const classes = useStyles();
    const [estadoCausal, setEstadoCausal] = useState<any>({
        razon: '',
        estado: '',
        causal: '',
        observaciones: ''
    });
    const [disabled, setDisabled] = useState(false)
    const InsertarEstadoCausalCliente = async (data: any, selectedEstado: any, selectedCausal: any) => {
        try {

            data.estado = selectedEstado?.descriptions;
            data.causal = selectedCausal?.causal;

            const response = await api.InsertarEstadoCausalCliente(data);
            return response;
        } catch (error) {
            toast.error('Error al agregar anotación');
            toast(`Error: ${error}`, { type: toast.TYPE.ERROR });
            throw error;
        }
    };

    const LevantarEstadoCausalCliente = async (data: any) => {
        try {
            data.causalEstadoId = 0;
            data.razon = '';

            const response = await api.LevantarEstadoCausalCliente(data);
            return response;
        } catch (error) {
            toast.error('Error al agregar razón');
            toast(`Error: ${error}`, { type: toast.TYPE.ERROR });
            throw error;
        }
    };



    // useEffect(() => {
    //     const validateEstados = () => {
    //         if (clientModel?.estadosCausaleslCliente?.length > 0) {
    //             setEstados(true)
    //         }
    //     }

    //     validateEstados()
    //     if(loginState.opciones?.filter(x=>x.codigo==='ACTUALIZAHVESTADOS').length>0){
    // 		setDisabled(false)
    // 	}else{
    // 		setDisabled(true)
    // 	}
    // });

    useEffect(() => {
        if (clientModel?.estadosCausaleslCliente?.length > 0) {
            setEstados(true)
        }

        if (loginState.opciones?.some(x => x.codigo === 'ACTUALIZAHVESTADOS')) {
            setDisabled(false)
        } else {
            setDisabled(true)
        }
    }, [clientModel, loginState]);

    const handleModalOpen = () => {
        setModalOpen(true);
    };

    const handleModalClose = () => {
        setModalOpen(false);
    };

    const handleModalRazonOpen = (id: number, activo: boolean) => {
        if (activo) {
            setIdModalRazon(id);
            setModalRazonOpen(true);
        }
    };

    const handleModalRazonClose = () => {
        setModalRazonOpen(false);
    };

    const actualizarEstados = (nuevosEstados: any[]) => {
        if (setClientModel) {
            setClientModel(prevState => ({
                ...prevState,
                estadosCausaleslCliente: nuevosEstados || []
            }));
        }
    };

    const actualizarRazon = (nuevaRazon: string) => {
        if (setClientModel) {
            setClientModel(prevState => ({
                ...prevState,
                razon: nuevaRazon
            }));
        }
    };

    const [selectedEstados, setSelectedEstados] = useState<number[]>([]);

    const handleCheckboxChange = (event: React.ChangeEvent<HTMLInputElement>, id: number) => {
        if (event.target.checked) {
            setSelectedEstados([...selectedEstados, id]);
        } else {
            setSelectedEstados(selectedEstados.filter(item => item !== id));
        }
    };

    const handleEliminarEstados = async () => {
        if (selectedEstados.length === 0) {
            return;
        }

        try {
            await Promise.all(
                selectedEstados.map(id =>
                    api.EliminarEstadoCausalCliente({ causalEstadoId: id })
                )
            );
            const estadosFiltrados = (clientModel?.estadosCausaleslCliente || []).filter(
                estado => !selectedEstados.includes(estado.causalEstadoId)
            );
            setSelectedEstados([]);
            swal({
                text: 'Estado(s) eliminado(s) correctamente, consultar nuevamente.',
                icon: 'success',
            });
            history.push('/HojaDeVida');
            actualizarEstados(estadosFiltrados);

        } catch (error) {
            toast.error('Error al eliminar el estado');
        }
    };



    return (
        <CHPaper title="Estado Actual Cliente">

            <Typography className={classes.titleform} variant="h5" component="h6">
                Estado Actual: {clientModel.estadoCliente}
            </Typography>
            {
                estados ? (
                    <table className="table">
                        <thead>
                            <tr>
                                <th className={classes.centeredHeader}>Acción</th>
                                <th className={classes.centeredHeader}>Causal</th>
                                <th className={classes.centeredHeader}>Fecha Registro</th>
                                <th className={classes.centeredHeader}>Fecha Retiro</th>
                                <th className={classes.centeredHeader}>Observaciones</th>
                                <th className={classes.centeredHeader}>Estado Cliente</th>
                                {/* <th className={classes.centeredHeader}>Razón</th> */}
                                <th className={classes.centeredHeader}>Estado</th>
                                <th className={classes.centeredHeader}> </th>
                            </tr>
                        </thead>
                        <tbody>
                            {clientModel?.estadosCausaleslCliente?.map((estado, index) => {
                                console.log("ESTADO COMPLETO:", estado);
                                return (
                                    <tr key={index}>
                                        <td>
                                            <CHCheckbox
                                                label=""
                                                checked={selectedEstados.includes(estado.causalEstadoId)}
                                                onChange={(event) => handleCheckboxChange(event, estado.causalEstadoId)}
                                                disabled={disabled}
                                            />
                                        </td>
                                        <td>{estado.descripcionCausal}</td>
                                        <td>{UtilService.getFullDate(estado.fecha)}</td>
                                        <td>{UtilService.getFullDate(estado.fechaRetiro)}</td>
                                        <td style={{ textTransform: 'uppercase' }}>{estado.observaciones}</td>
                                        <td>{estado.nombreEstado}</td>
                                        {/* <td>{estado.razon}</td> */}
                                        <td>{estado.estado ? 'ACTIVO' : 'INACTIVO'}</td>
                                        <td className={classes.centeredHeader}>
                                            <Button
                                                variant="contained"
                                                color="primary"
                                                onClick={() => handleModalRazonOpen(estado.causalEstadoId, estado.estado)}
                                                disabled={disabled}
                                                className={disabled ? '' : classes.button}
                                            >
                                                Levantar estado
                                            </Button>
                                        </td>
                                    </tr>
                                )

                            })}
                        </tbody>
                    </table >
                ) : 'El afiliado no cuenta con cambios de estados'
            }
            <div className="buttons">
                <>
                    <Button
                        className={disabled ? '' : classes.button}
                        variant="outlined"
                        type="submit"
                        onClick={handleEliminarEstados}
                        disabled={disabled}
                    >
                        ELIMINAR
                    </Button>
                    <Button
                        className={disabled ? '' : classes.button}
                        variant="outlined"
                        onClick={handleModalOpen}
                        disabled={disabled}
                    >
                        ADICIONAR
                    </Button>
                </>
            </div>
            <PopupEstadoCliente
                open={modalOpen}
                handleModalClose={handleModalClose}
                InsertarEstadoCausalCliente={InsertarEstadoCausalCliente}
                updateEstados={actualizarEstados}
                clientModel={clientModel}
            />
            <PopupRazonCausal
                open={modalRazonOpen}
                handleModalRazonClose={handleModalRazonClose}
                clientModel={clientModel}
                LevantarEstadoCausalCliente={LevantarEstadoCausalCliente}
                updateRazon={actualizarRazon}
                causaEstadoId={idModalRazon}
            />
        </CHPaper >)
}


export default connect()(Estados);