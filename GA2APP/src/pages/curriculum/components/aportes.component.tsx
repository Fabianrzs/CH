import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles, Typography, } from '@material-ui/core';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';


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
    }),
);

function Aportes({
    catalogState,
    clientModel,
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
}) {

    const [aportes, setAportes] = useState<boolean>(false);
    const classes = useStyles();
    useEffect(() => {
        const validateAportes = () => {
            if (Array.isArray(clientModel?.aportesCategoria) && clientModel.aportesCategoria.length !== 0) {
                setAportes(true);
            }
        };

        validateAportes();
    }, [clientModel]);


    return (
        <CHPaper title="Aportes Categoría">

            <Typography className={classes.titleform} variant="h5" component="h6">
                Aportes
            </Typography>
            {
                aportes ? (
                    <table className="table">
                        <thead>
                            <tr>
                                <th>Categoría</th>
                                <th>Cuotas</th>
                                <th>Primera Cuota</th>
                                <th>Última Cuota</th>
                            </tr>
                        </thead>

                        <tbody>
                            {clientModel?.aportesCategoria?.map((element, index) => (

                                <tr key={`table-scheduleAportes${index}`}>
                                    <td className='dateReport3'>{catalogState.categories.find(x => x.id === element.ctG_ID)?.descripcion}</td>
                                    <td className='dateReport3'>{element.apC_CUOTAS_ACUMULADAS}</td>
                                    <td className='dateReport3'>{UtilService.getDate(element.apC_FECHA_PRIMERA_CUOTA)}</td>
                                    <td className='dateReport3'>{UtilService.getDate(element.apC_FECHA_ULTIMA_COUTA)}</td>

                                </tr>

                            )
                            )}
                        </tbody>
                    </table>
                ) : 'El afiliado no cuenta con aportes de categorías'
            }
        </CHPaper>)
}


export default connect()(Aportes);