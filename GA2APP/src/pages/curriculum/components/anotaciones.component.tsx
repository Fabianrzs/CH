import React, { useEffect, useState, Dispatch } from 'react';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { Controller } from 'react-hook-form';
import { Grid, makeStyles, createStyles, Typography, Button, Checkbox } from '@material-ui/core';
import swal from 'sweetalert';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import { PopupObservacionesAnotaciones } from './PopupObservacionesAnotaciones';
import ApiClients from '../../../services/api-clients.service';
import AuthService from '../../../services/auth.service';
import history from '../../../utils/history.router';
import { LoginState } from '../../../reducers/login.reducer';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../../actions/client.action';


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


function Anotaciones({
    catalogState,
    clientModel,
    loginState,
    setClientModel
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
    setClientModel: React.Dispatch<React.SetStateAction<ClienteDto>> | undefined;
    loginState: LoginState;
}) {
    const api = ApiClients();
    const classes = useStyles();
    const user = AuthService.getUserInfo();
    const [modalOpen, setModalOpen] = useState(false);
    const [selectedAnotaciones, setSelectedAnotaciones] = useState<string[]>([]);
    const [disabled, setDisabled] = useState(false)
    useEffect(() => {

        if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVANOTACIONES').length > 0) {
            setDisabled(false)
        } else {
            setDisabled(true)
        }
    });
    const handleCheckboxChange = (event: React.ChangeEvent<HTMLInputElement>, id: string) => {
        if (event.target.checked) {
            setSelectedAnotaciones([...selectedAnotaciones, id]);
        } else {
            setSelectedAnotaciones(selectedAnotaciones.filter(item => item !== id));
        }
    };

    const handleModalOpen = () => {
        setModalOpen(true);
    };

    const handleModalClose = () => {
        setModalOpen(false);
    };

    const actualizarAnotaciones = (nuevasAnotaciones: any[]) => {
        if (setClientModel) {
            setClientModel(prev => ({ ...prev, anotaciones: nuevasAnotaciones }));
        }
    };


    const appendAnotacion = (nueva: any) => {
        setAnotaciones(prev =>
            [nueva, ...prev].sort(
                (a, b) => new Date(b.fechA_ANOTACION).getTime() - new Date(a.fechA_ANOTACION).getTime()
            )
        );
    };


    const handleEliminarAnotaciones = async () => {
        if (selectedAnotaciones.length === 0) {
            return;
        }

        try {
            await Promise.all(
                selectedAnotaciones.map((id: any) =>
                    api.EliminarAnotacionCliente({ AnotacionClienteId: id })
                )
            );
            // const anotacionesFiltradas = clientModel.anotaciones.filter((_, index) => !selectedAnotaciones.includes(index.toString()));
            const anotacionesFiltradas = clientModel.anotaciones
                .filter(a => !selectedAnotaciones.includes(String(a.anotacioN_ID)));


            setSelectedAnotaciones([]);
            swal({
                text: 'Anotacion(es) eliminada(s) correctamente, consultar nuevamente.',
                icon: 'success',
            });
            history.push('/HojaDeVida');
            if (setClientModel) {
                setClientModel(prevState => ({ ...prevState, anotaciones: anotacionesFiltradas }));
            }
        } catch (error) {
            toast.error('Error al eliminar anotaciones');
        }
    };

    // const actualizarAnotaciones = (nuevasAnotaciones: any[]) => {
    //     if (setClientModel) {
    //         setClientModel(prevState => ({
    //             ...prevState,
    //             anotaciones: nuevasAnotaciones
    //         }));
    //     }
    // };

    const [anotaciones, setAnotaciones] = useState<any[]>(clientModel.anotaciones ?? []);

    useEffect(() => {
        setAnotaciones(clientModel.anotaciones ?? []);
    }, [clientModel.anotaciones]);




    return (
        <CHPaper title="Anotaciones Cliente">
            <Typography className={classes.titleform} variant="h5" component="h6">
                Anotaciones
            </Typography>
            {anotaciones.length > 0 ? (
                <table className="credit-table">
                    <thead>
                        <tr>
                            <th className={classes.centeredHeader}>Acción</th>
                            <th className={classes.centeredHeader}>Fecha anotación</th>
                            <th className={classes.centeredHeader}>Observación</th>
                            <th className={classes.centeredHeader}>Usuario</th>
                        </tr>
                    </thead>
                    <tbody>
                        {anotaciones.map((element, index) => (
                            <tr key={`table-scheduleAportes${index}`}>
                                <td>
                                    {element.anotacioN_ID !== 0 && (
                                        <Checkbox
                                            checked={selectedAnotaciones.includes(String(element.anotacioN_ID))}
                                            onChange={(event) => handleCheckboxChange(event, String(element.anotacioN_ID))}
                                            color="primary"
                                            inputProps={{ 'aria-label': `checkbox ${index}` }}
                                            disabled={disabled}
                                        />
                                    )}
                                </td>
                                <td className='dateReport3'>{UtilService.getFullDate(element.fechA_ANOTACION)}</td>
                                <td className='dateReport3'>{element.anotacion}</td>
                                <td className='dateReport3'>{element.usuario}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <div>El afiliado no cuenta con anotaciones</div>
            )}
            <div className="buttons">
                <>
                    <Button
                        variant="outlined"
                        onClick={handleEliminarAnotaciones}
                        disabled={disabled ? true : selectedAnotaciones.length === 0}
                        className={disabled ? '' : classes.button}
                    >
                        ELIMINAR
                    </Button>
                    <Button
                        className={disabled ? '' : classes.button}
                        variant="outlined"
                        onClick={handleModalOpen}
                    // disabled={disabled}
                    >
                        ADICIONAR
                    </Button>
                    <PopupObservacionesAnotaciones open={modalOpen} handleModalClose={handleModalClose} clientModel={clientModel} updateAnotaciones={actualizarAnotaciones} appendAnotacion={appendAnotacion} />
                </>
            </div>
        </CHPaper>
    );
}

export default connect()(Anotaciones);
