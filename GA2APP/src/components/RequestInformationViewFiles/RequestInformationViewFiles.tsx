import React, { useEffect, useState } from 'react';
import {
    Button,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { CreditFileModel } from '../../models/credit-file.model';
import { CreditFilesRequestModel } from '../../models/requests/credit-files-request.model';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import CHPaper from '../ch-paper/CHPaper';
import { AppState } from '../../store/rootReducer';
import {
    startGetPruebaData,
    startPostCreateSimulation,
    startPostCreditFilesData,
} from '../../actions/credit.actions';
import CreditApi from '../../services/api-credit.service';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';
import AuthService from '../../services/auth.service';
import { CreditBasicDataModel } from '../../models/credit-basic-data.model';

const api = CreditApi();

function RequestInformationViewFiles({
    isLoading,
    creditFilesData,
    startPostCreditFilesData,
    procedureRequest,
    creditBasicData,
}: {
    isLoading: boolean;
    creditFilesData: CreditFileModel[];
    startPostCreditFilesData: (data: CreditFilesRequestModel) => void;
    procedureRequest: ProcedureRequestModel;
    creditBasicData: CreditBasicDataModel;
}) {
    const user = AuthService.getUserInfo();

    const [isLoadingFile, setIsLoadingFile] = useState(false);
    useEffect(() => {
        const data: CreditFilesRequestModel = {
            soC_ID: procedureRequest.requestID,
            tcR_ID: procedureRequest.typeCreditID,
            afL_ID: procedureRequest.flowActivityID,
        };
        startPostCreditFilesData(data);
    }, []);

    const datos = {
        reporte: 'REPORTE_HABERES',
        clienteIdentificacion:
            procedureRequest.clientIdentificationNumber.toString(),
        cuentaId: 0,
    };

    const datosFiRiesgo = {
        idSolicitud: procedureRequest.requestID,
        nombreSolicitante: `${user.USR_NOMBRE}${user.USR_SEGUNDONOMBRE}${user.USR_APELLIDO}${user.USR_SEGUNDOAPELLIDO}`,
        idSolicitante: user.USR_IDENTIFICACION,
    };

    const datosLeasing: any = {
        idSolicitud: procedureRequest.requestID,
    };

    const getFileLeasing = (path: string) => async () => {
        try {
            setIsLoadingFile(true);
            const data = await api.postCreditDownloadFileLeasing(datosLeasing);
            const ext = UtilService.getFileExtension(path);
            if (ext) {
                const mimeType = UtilService.getMimeTypeFromExtension(ext);
                if (mimeType) {
                    const file = new Blob([data], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                }
            }
            setIsLoadingFile(false);
        } catch (e) {
            await NotificationService.genericAlert(
                'Advertencia',
                'No se puede generar archivo para este Usuario.',
                'warning',
            );
            setIsLoadingFile(false);
        }
    };

    const getFileS = (path: string) => async () => {
        if (path === '\\Reporte_Comercial.pdf') {
            const request = {
                idType: creditBasicData.tidID,
                idNumber: procedureRequest.clientIdentificationNumber,
            };
            try {
                setIsLoadingFile(true);
                const data = await api.postObtenerHistorialComercialBuro(
                    request,
                );
                const ext = UtilService.getFileExtension(path);
                if (ext) {
                    const mimeType = UtilService.getMimeTypeFromExtension(ext);
                    if (mimeType) {
                        const file = new Blob([data], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);

                        window.open(fileURL);
                    }
                }
                setIsLoadingFile(false);
            } catch (e) {
                await NotificationService.genericAlert(
                    'Advertencia',
                    'No se puede generar archivo  para este Usuario.',
                    'warning',
                );
                setIsLoadingFile(false);
            }
        } else {
            try {
                setIsLoadingFile(true);
                const data = await api.postCreditDownloadFileS(path);
                const ext = UtilService.getFileExtension(path);
                if (ext) {
                    const mimeType = UtilService.getMimeTypeFromExtension(ext);
                    if (mimeType) {
                        const file = new Blob([data], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL);
                    }
                }
                setIsLoadingFile(false);
            } catch (e) {
                await NotificationService.genericAlert(
                    'Advertencia',
                    'No se puede generar archivo para este Usuario.',
                    'warning',
                );
                setIsLoadingFile(false);
            }
        }
    };

    const getFile = (path: string) => async () => {
        try {
            setIsLoadingFile(true);
            const data = await api.postCreditDownloadFile(datos);
            const ext = UtilService.getFileExtension(path);
            if (ext) {
                const mimeType = UtilService.getMimeTypeFromExtension(ext);
                if (mimeType) {
                    const file = new Blob([data], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                }
            }
            setIsLoadingFile(false);
        } catch (e) {
            await NotificationService.genericAlert(
                'Advertencia',
                'No se puede generar archivo para este Usuario.',
                'warning',
            );
            setIsLoadingFile(false);
        }
    };

    const getFileReporteFiRiesgo = (path: string) => async () => {
        try {
            setIsLoadingFile(true);

            const data = await api.postCreditDownloadFileFiRiesgo(
                datosFiRiesgo,
            );
            const ext = UtilService.getFileExtension(path);
            if (ext) {
                const mimeType = UtilService.getMimeTypeFromExtension(ext);
                if (mimeType) {
                    const file = new Blob([data], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);

                    window.open(fileURL);
                }
            }
            setIsLoadingFile(false);
        } catch (e) {
            await NotificationService.genericAlert(
                'Advertencia',
                'No se puede generar archivo  para este Usuario.',
                'warning',
            );
            setIsLoadingFile(false);
        }
    };

    if (isLoading || isLoadingFile) {
        return <CHLoadingProcess />;
    }

    console.log('datos', procedureRequest);

    return (
        <CHPaper title="Archivos Solicitudes">
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell component="th">
                            <b>Tipo Documento</b>
                        </TableCell>
                        <TableCell component="th">
                            <b>Acción</b>
                        </TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {creditFilesData.map((file, key) => (
                        <TableRow key={`file-${key}`}>
                            <TableCell>{file.docType}</TableCell>
                            
                            <TableCell>
                                {file.docID === 1 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 2 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 3 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 4 && (
                                    <Button onClick={getFile(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 5 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 6 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 7 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 8 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 9 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 10 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 11 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 12 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 13 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 14 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 15 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 16 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 17 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 18 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 19 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 20 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 21 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 22 && (
                                    <Button
                                        onClick={getFileLeasing(file.imagePath)}
                                    >
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 23 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 24 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 25 && (
                                    <Button onClick={getFileS(file.imagePath)}>
                                        Ver
                                    </Button>
                                )}
                                {file.docID === 26 && (
                                    <Button
                                        onClick={getFileReporteFiRiesgo(
                                            file.imagePath,
                                        )}
                                    >
                                        Ver
                                    </Button>
                                )}
                                   {file.docID === 61 && (
                                    <Button
                                        onClick={getFileS(
                                            file.imagePath,
                                        )}
                                    >
                                        Ver
                                    </Button>
                                )}
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </CHPaper>
    );
}

const mapStateToProps = (state: AppState) => ({
    // isLoading: state.credit.loading,
    creditFilesData: state.credit.creditFilesData,
    creditBasicData: state.credit.creditBasicData,
});

const mapDispatchToProps = (dispatch: any) => ({
    startPostCreditFilesData: (request: any) =>
        dispatch(startPostCreditFilesData(request)),
    startPPruebaData: (request: any) => dispatch(startGetPruebaData(request)),

    startProcess: (request: any) =>
        dispatch(startPostCreateSimulation(request)),
});

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationViewFiles);
