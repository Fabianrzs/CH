
import React from "react";
import { Button, createStyles, makeStyles } from "@material-ui/core";
import { ProcedureRequestModel } from "../../models/procedure-request.model";
import UtilService from "../../utils/util.service";

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
    }),
);


function CreditTable({
    data,
    onClickItem,
    isDataLoaded,
}: {
    data: ProcedureRequestModel[];
    onClickItem: (data: ProcedureRequestModel) => void;
    isDataLoaded: boolean;
}) {
   
    const classes = useStyles();

    if (!isDataLoaded) {
        return null;
    }

    if (data.length === 0 && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }

    return (
        <table className="credit-table">
            <thead>
                <tr>
                    <th>Acción</th>
                    <th>No. Radicado</th>
                    <th>No. Solicitud</th>
                    <th>Tipo de Crédito</th>
                    <th>No. Documento Cliente</th>
                    <th>Nombre / Razón Social</th>
                    <th>Fecha Solicitud</th>
                    <th>Valor Crédito</th>
                    <th>Actividad Actual</th>
                    <th>Estado</th>
                </tr>
            </thead>
            <tbody>
            {data
                    ?.sort((a, b) => a.requestNumber - b.requestNumber || a.numerO_SOLICITUD - b.numerO_SOLICITUD) // Ordenar por No. Solicitud
                    .map((row, index) => (
                        <tr key={`table-${index}`}>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => onClickItem(row)}
                                >
                                   {row?.stateActivity === 'FINALIZADA' ||  row?.stateActivity === 'RECHAZADA'? 'Detalle' : 'Procesar'} 
                                </Button>
                            </td>
                            <td>{row.numerO_RADICADO || 'No Aplica'}  </td>
                            <td>{row?.requestNumber || row?.numerO_SOLICITUD}</td>
                            <td>{row?.creditType || row?.tipO_CREDITO}</td>
                            <td>{row?.clientIdentificationNumber || row?.cedulA_CLIENTE}</td>
                            <td>{row?.clientName || row?.nombrE_CLIENTE}</td>
                            <td>{UtilService.getDate(row?.requestDate || row?.fechA_SOLICITUD)}</td>
                            <td>{UtilService.formatCurrency(row?.creditValue || row?.valoR_CREDITO)}</td>
                            <td>{row.activity || row?.actividad}</td>
                            <td>{row.stateActivity}</td>
                        </tr>
                    ))}
            </tbody>
        </table>
    );
}

export default CreditTable