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

function Embargos({
    clientModel,
    catalogState
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
}) {
    const classes = useStyles();

    // const embargosFiltrados = clientModel?.solicitudes?.filter(s => {
    //     const nombre = s.tipoSolicitud?.toUpperCase() ?? '';
    //     return nombre.includes('EMBARGOS') || nombre.includes('DESEMBARGOS') || nombre.includes('EMBARGO');
    // });

    const embargosFiltrados = clientModel?.embargosAfiliado?.length
        ? clientModel.embargosAfiliado
        : [];

    useEffect(() => {
        console.log('client', clientModel);

    }, []);


    return (
        <CHPaper title="Embargos">

            <Typography className={classes.titleform} variant="h5" component="h6">
                Embargos
            </Typography>

            <table className="table">
                <thead>
                    <tr>
                        <th>Radicado</th>
                        <th>Solicitud</th>
                        <th>Estado Solicitud</th>
                        <th>Estado Bloqueo</th>
                        <th>Código Juzgado</th>
                        <th>Razón</th>
                        <th>Monto/Porcentaje</th>
                    </tr>
                </thead>

                <tbody>
                    {embargosFiltrados?.map((element, index) => (
                        
                        <tr key={`table-embargo-${index}`}>
                            <td className="dateReport3">{element.radicado}</td>

                            <td className="dateReport3">{element.solicitud}</td>

                            <td className="dateReport3">{element.estadO_SOLICITUD}</td>

                            <td className="dateReport3">{element.estadO_BLOQUEO}</td>

                            <td className="dateReport3">{element.codigO_JUZGADO}</td>

                            <td className="dateReport3">{element.razon}</td>

                            <td className="dateReport3">
                                {Number(element.montoporcentaje) > 0 && Number(element.montoporcentaje) < 100
                                    ? `${element.montoporcentaje}%`
                                    : UtilService.formatCurrency(element.montoporcentaje)}
                            </td>
                        </tr>
                    ))}
                </tbody>

            </table>

        </CHPaper>)
}


export default connect()(Embargos);
