import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles, } from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import AddNoveltyOperations from './addNoveltyOperations';
import UtilService from '../../../utils/util.service';
import { startGetOperacionesBloqueos } from '../../../actions/realStateRegistrations.action';
import { startGetCausalBlockage, startGetRequestTypeRealStateRegistration } from '../../../actions/catalog.action';
import { HistorialNovedadesMatriculasInmobiliarias } from '../../../types/realStateRegistrations.type';

const useStyles = makeStyles(() =>
    createStyles({
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '16%',
            marginLeft: '83%',
            marginTop: '2%',
        },
        icon: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#42b7ef',
            margin: '10px'
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
        },
        observaciones: {
            textAlign: 'justify',
            whiteSpace: 'pre-line',
            paddingBottom: '20px',
            paddingRight: '12px',
        }
    }),
);

function propietaryHistory({
    realStateRegistrationsState,
    startGetOperacionesBloqueos,
    startGetCausalBlockage,
    startGetRequestTypeRealStateRegistration,
    consultar,
}: {
    realStateRegistrationsState: RealStateRegistrationsState;
    startGetOperacionesBloqueos: () => void;
    startGetCausalBlockage: () => void;
    startGetRequestTypeRealStateRegistration: () => void;
    consultar: () => void;
}): any {
    
    const [open, setOpen] = useState(false);
    const classes = useStyles();
    const [ultimoRegistro, setUltimoRegistro] = useState<HistorialNovedadesMatriculasInmobiliarias | any>(null);
    const handleOpenAddress = () => {
        startGetOperacionesBloqueos();
        setOpen(true);
        if (!realStateRegistrationsState?.informacionBasica?.novedades || realStateRegistrationsState.informacionBasica.novedades.length === 0) {
            setUltimoRegistro(null);
            consultar();// Establecer el último registro como null o algún valor predeterminado en caso de que no haya elementos en 'novedades'
        } else {
            const ultimoRegistro = realStateRegistrationsState.informacionBasica.novedades[0];
            setUltimoRegistro(ultimoRegistro);
            consultar();
        }
    };

    useEffect(() => {
        startGetOperacionesBloqueos();
        startGetRequestTypeRealStateRegistration();
        startGetCausalBlockage();
        consultar();
    }, []);

    const onClose = () => {
        setOpen(false);
        consultar();
    };

    return (
        <Grid container>
            <CHPaperNew>
                {realStateRegistrationsState?.informacionBasica?.novedades?.length > 0 ? (
                    <table className="table">
                        <thead>
                            <tr>
                                <th className='tipoOperacion1'>Tipo operación</th>
                                <th className='tipoOperacion1'>Causal</th>
                                <th className='tipoOperacion1'>Fecha operación</th>
                                <th className='tipoOperacion1'>Observaciones</th>
                                <th className='tipoOperacion1'>Creado por</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                realStateRegistrationsState?.informacionBasica?.novedades?.map((row, index) => (
                                    <tr key={`table-${index}`}>
                                        {row.tipoOperacion !== "DESBLOQUEO" ?
                                            <td className="tab-space-01">{row?.observaciones === 'BLOQUEO AUTOMATICO' || row?.observaciones?.includes('COMPORTAMIENTO INUSUAL EN LA NUMERACIÓN') ? 'BLOQUEO AUTOMATICO' : 'BLOQUEO MANUAL'}</td>
                                            :
                                            <td className="tab-space-01">{row.tipoOperacion}</td>
                                        }
                                        <td className="tab-space-01">{row.causal}</td>
                                        <td className="tab-space-01">{UtilService.getFullDateWithTimeDirection(row.fechaNovedad)}</td>
                                        <td className={classes.observaciones}>{row.observaciones}</td>
                                        <td className="tab-space-01">{row.createPor}</td>
                                    </tr>
                                ))}
                        </tbody>
                    </table>
                ) : 'La matrícula inmobiliaria no tiene histórico de novedades'}

                <Button className={classes.processButton} onClick={handleOpenAddress}>
                    AGREGAR NOVEDAD
                </Button>
            </CHPaperNew>
            <AddNoveltyOperations
                isOpen={open}
                ultimoRegistro={ultimoRegistro}
                onClose={onClose}
                consultar={consultar}
            />
        </Grid>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    realStateRegistrationsState: state.realStateRegistrations,
});

const mapDispatchToProps = {
    startGetOperacionesBloqueos,
    startGetCausalBlockage,
    startGetRequestTypeRealStateRegistration,
}

export default connect(mapStateToProps, mapDispatchToProps)(propietaryHistory);