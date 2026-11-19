import React, { useEffect } from 'react';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import { CreditObservationModel } from '../../models/credit-observation.model';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import CHPaper from '../ch-paper/CHPaper';
import { CreditObservationsRequestModel } from '../../models/requests/credit-observations.request.model';
import UtilService from '../../utils/util.service';

function RequestInformationObservations({
    procedureRequest,
    observationsData,
    isLoading,
    startPostCreditObservationsData,
}: {
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean;
    observationsData: CreditObservationModel[];
    startPostCreditObservationsData: (
        data: CreditObservationsRequestModel,
    ) => void;
}) {
    useEffect(() => {
        const data: CreditObservationsRequestModel = {
            trS_ID: procedureRequest.trsID,
            ind: 1,
        };
        startPostCreditObservationsData(data);
    }, []);
    if (isLoading) {
        return <CHLoadingProcess />;
    }
    return (
        <CHPaper title="Observaciones">
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell component="th">ACTIVIDAD</TableCell>
                        <TableCell component="th">USUARIO</TableCell>
                        <TableCell component="th">ESTADO</TableCell>
                        <TableCell component="th">OBSERVACIÓN</TableCell>
                        <TableCell component="th">FECHA INGRESO</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {observationsData.map((observation, key) => (
                        <TableRow key={`observation-${key}`}>
                            <TableCell>{observation.activity}</TableCell>
                            <TableCell>{observation.userName}</TableCell>
                            <TableCell>
                                {observation.descripcion}
                            </TableCell>
                            <TableCell>
                                {observation.observation.toUpperCase()}
                            </TableCell>
                            <TableCell>
                                {UtilService.getFullDateNew(observation.entryDate)}
                            </TableCell>
                        </TableRow>
                    ))}
                    {observationsData.length === 0 && (
                        <TableRow>
                            <TableCell colSpan={3}>
                                No hay observaciones
                            </TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </CHPaper>
    );
}

export default RequestInformationObservations;
