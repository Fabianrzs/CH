import React, {
    ChangeEvent,
    useEffect,
    useState,
} from 'react';
import { connect } from 'react-redux';
import {
    IconButton,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import {
    DeleteOutline,
    Visibility,
} from '@material-ui/icons';
import { toast } from 'react-toastify';
import CHPaper from '../ch-paper/CHPaper';
import { AppState } from '../../store/rootReducer';
import { startPostCreditFlowDocuments } from '../../actions/credit.actions';
import { CreditFlowDocumentsRequestModel } from '../../models/requests/credit-flow-documents-request.model';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import { CreditFlowDocumentModel } from '../../models/credit-flow-document.model';
import CreditApi from '../../services/api-credit.service';
import CHFileUploader from '../ch-file-uploader/CHFileUploader';
import NotificationService from '../../services/notification.service';
import UtilService from '../../utils/util.service';
import { CreditBasicDataModel } from '../../models/credit-basic-data.model';
import CHCheckbox from '../ch-checkbox/CHCheckbox';
import { LoadingComponent } from '../loading/loading.component';
import AuthService from '../../services/auth.service';

const api = CreditApi();

type DocumentoTemporal = {
    idTipoDocumento: number;
    archivo: File;
    rutaImagen: string;
    creadoPor: string;
    fechaCreacion: string;
    numero_documento: string;
};

function RequestInformationUploadFiles({
    typeRequest,
    procedureRequest,
    creditFlowDocumentsData,
    creditBasicData,
    docObligatorio,
    isLoading,
    documentosTemporales,
    setDocumentosTemporales,
}: {
    typeRequest?: string;
    procedureRequest: ProcedureRequestModel;
    creditFlowDocumentsData: CreditFlowDocumentModel[];
    creditBasicData: CreditBasicDataModel;
    docObligatorio: any;
    isLoading: boolean;
    documentosTemporales: DocumentoTemporal[];
    setDocumentosTemporales: React.Dispatch<
        React.SetStateAction<DocumentoTemporal[]>
    >;
}) {
    const [
        filesUploaded,
        setFilesUploaded,
    ] = useState<number[]>([]);

    const [
        isLoadingFile,
        setIsLoadingFile,
    ] = useState<boolean>(false);

    const [
        documentosSeguros,
        setDocumentosSeguros,
    ] = useState<any[]>([]);

    const [
        documentosVendedor,
        setDocumentosVendedor,
    ] = useState<any[]>([]);

    const [
        documentosInmueble,
        setDocumentosInmueble,
    ] = useState<any[]>([]);

    const [
        documentosAfiliado,
        setDocumentosAfiliado,
    ] = useState<any[]>([]);

    useEffect(() => {
        listarArchivos();
    }, [filesUploaded]);

    const listarArchivos = async () => {
        const request: CreditFlowDocumentsRequestModel = {
            tcR_ID:
                procedureRequest.creditTypeID,
            orden:
                procedureRequest.order,
            principal:
                procedureRequest.principalActivity,
            soC_ID:
                procedureRequest.requestID,
            afL_ID:
                procedureRequest.flowActivityID,
        };

        try {
            const response =
                await api.postCreditFiles(
                    request,
                );

            if (!response) {
                return;
            }

            if (
                !response.isSuccess ||
                !response.data
            ) {
                await NotificationService.genericAlert(
                    'Error en actualización',
                    'Ocurrió un error al intentar actualizar, intente de nuevo.',
                    'error',
                );

                return;
            }

            const docOrdenF = [
                ...response.data.filter(
                    (x: any) =>
                        x.obligatorio ===
                        'S',
                ),

                ...response.data.filter(
                    (x: any) =>
                        x.obligatorio ===
                        'N',
                ),
            ];

            const docSeguro =
                docOrdenF.filter(
                    (x: any) =>
                        x.tdC_ID === 65 ||
                        x.tdC_ID === 64 ||
                        x.tdC_ID === 21 ||
                        x.tdC_ID === 20 ||
                        x.tdC_ID === 23 ||
                        x.tdC_ID === 24 ||
                        x.tdC_ID === 5 ||
                        x.tdC_ID === 75 ||
                        x.tdC_ID === 159 ||
                        x.tdC_ID === 170,
                );

            setDocumentosSeguros(
                docSeguro,
            );

            const docVendedor =
                docOrdenF.filter(
                    (x: any) =>
                        x.tdC_ID === 66 ||
                        x.tdC_ID === 67 ||
                        x.tdC_ID === 15 ||
                        x.tdC_ID === 16,
                );

            setDocumentosVendedor(
                docVendedor,
            );

            const docInmueble =
                docOrdenF.filter(
                    (x: any) =>
                        x.tdC_ID === 9 ||
                        x.tdC_ID === 61 ||
                        x.tdC_ID === 6 ||
                        x.tdC_ID === 10 ||
                        x.tdC_ID === 11 ||
                        x.tdC_ID === 12 ||
                        x.tdC_ID === 13 ||
                        x.tdC_ID === 130 ||
                        x.tdC_ID === 131 ||
                        x.tdC_ID === 115 ||
                        x.tdC_ID === 78 ||
                        x.tdC_ID === 160 ||
                        x.tdC_ID === 108,
                );

            setDocumentosInmueble(
                docInmueble,
            );

            const docAfiliado =
                docOrdenF.filter(
                    (x: any) =>
                        x.tdC_ID !== 9 &&
                        x.tdC_ID !== 67 &&
                        x.tdC_ID !== 61 &&
                        x.tdC_ID !== 64 &&
                        x.tdC_ID !== 65 &&
                        x.tdC_ID !== 66 &&
                        x.tdC_ID !== 21 &&
                        x.tdC_ID !== 20 &&
                        x.tdC_ID !== 23 &&
                        x.tdC_ID !== 24 &&
                        x.tdC_ID !== 6 &&
                        x.tdC_ID !== 10 &&
                        x.tdC_ID !== 11 &&
                        x.tdC_ID !== 12 &&
                        x.tdC_ID !== 13 &&
                        x.tdC_ID !== 15 &&
                        x.tdC_ID !== 16 &&
                        x.tdC_ID !== 5 &&
                        x.tdC_ID !== 130 &&
                        x.tdC_ID !== 131 &&
                        x.tdC_ID !== 115 &&
                        x.tdC_ID !== 75 &&
                        x.tdC_ID !== 170 &&
                        x.tdC_ID !== 108 &&
                        x.tdC_ID !== 160 &&
                        x.tdC_ID !== 78,
                );

            setDocumentosAfiliado(
                docAfiliado,
            );

            docObligatorio(
                response.data,
            );
        } catch {
            toast(
                'Error del sistema',
                {
                    type:
                        toast.TYPE.ERROR,
                },
            );
        }
    };

    const getFileS =
        (path: string) =>
        async () => {
            if (
                path ===
                '\\Reporte_Comercial.pdf'
            ) {
                const request = {
                    idType:
                        creditBasicData.tidID,

                    idNumber:
                        procedureRequest.clientIdentificationNumber,
                };

                try {
                    setIsLoadingFile(
                        true,
                    );

                    const data =
                        await api.postObtenerHistorialComercialBuro(
                            request,
                        );

                    const ext =
                        UtilService.getFileExtension(
                            path,
                        );

                    if (!ext) {
                        return;
                    }

                    const mimeType =
                        UtilService.getMimeTypeFromExtension(
                            ext,
                        );

                    if (!mimeType) {
                        return;
                    }

                    const file =
                        new Blob(
                            [data],
                            {
                                type:
                                    mimeType,
                            },
                        );

                    const fileURL =
                        URL.createObjectURL(
                            file,
                        );

                    window.open(
                        fileURL,
                        '_blank',
                    );

                    window.setTimeout(
                        () => {
                            URL.revokeObjectURL(
                                fileURL,
                            );
                        },
                        1000,
                    );
                } catch {
                    await NotificationService.genericAlert(
                        'Advertencia',
                        'No se puede generar archivo para este usuario.',
                        'warning',
                    );
                } finally {
                    setIsLoadingFile(
                        false,
                    );
                }

                return;
            }

            try {
                setIsLoadingFile(
                    true,
                );

                const data =
                    await api.postCreditDownloadFileS(
                        path,
                    );

                const ext =
                    UtilService.getFileExtension(
                        path,
                    );

                if (!ext) {
                    return;
                }

                const mimeType =
                    UtilService.getMimeTypeFromExtension(
                        ext,
                    );

                if (!mimeType) {
                    return;
                }

                const file =
                    new Blob(
                        [data],
                        {
                            type:
                                mimeType,
                        },
                    );

                const fileURL =
                    URL.createObjectURL(
                        file,
                    );

                window.open(
                    fileURL,
                    '_blank',
                );

                window.setTimeout(
                    () => {
                        URL.revokeObjectURL(
                            fileURL,
                        );
                    },
                    1000,
                );
            } catch {
                await NotificationService.genericAlert(
                    'Advertencia',
                    'No se puede generar archivo para este usuario.',
                    'warning',
                );
            } finally {
                setIsLoadingFile(
                    false,
                );
            }
        };

   
    const uploadFile =
        (id: any) =>
        async (
            event: ChangeEvent<HTMLInputElement>,
        ) => {
            const archivo =
                event.target.files?.[0];

            if (!archivo) {
                return;
            }

            const esPdf =
                archivo.type ===
                    'application/pdf' ||
                archivo.name
                    .toLowerCase()
                    .endsWith(
                        '.pdf',
                    );

            if (!esPdf) {
                await NotificationService.genericAlert(
                    'Error',
                    'Solo se permite cargar archivos PDF',
                    'error',
                );

                event.target.value =
                    '';

                return;
            }

            try {
                setIsLoadingFile(
                    true,
                );

                const user =
                    AuthService.getUserInfo();

                if (!user?.USR_ID) {
                    await NotificationService.genericAlert(
                        'Error',
                        'No fue posible obtener el usuario que realiza el cargue.',
                        'error',
                    );

                    return;
                }

                const formData =
                    new FormData();

                formData.append(
                    'request',
                    archivo,
                );

                /*
                 * ENDPOINT 1:
                 * /api/credito/cargaDocumentoSolicCredito
                 *
                 * Nombre real del servicio:
                 * postCreditUploadDocument
                 */
                const respuestaCarga =
                    await api.postCreditUploadDocument(
                        formData,
                    );

                if (
                    !respuestaCarga?.isSuccess ||
                    !respuestaCarga?.data
                ) {
                    await NotificationService.genericAlert(
                        'Error',
                        respuestaCarga?.returnMessage ||
                            'No fue posible cargar el archivo.',
                        'error',
                    );

                    return;
                }

                const rutaImagen =
                    String(
                        respuestaCarga.data,
                    );

                const fechaCreacion =
                    new Date().toISOString();

                const numeroDocumento =
                    String(
                        procedureRequest
                            .clientIdentificationNumber ||
                            '',
                    );

                const payloadDocumento =
                    {
                        creadoPor:
                            user.USR_ID,

                        idSolicitudCredito:
                            procedureRequest.requestID,

                        idTipoDocumento:
                            Number(id),

                        rutaImagen,

                        fechaCreacion,

                        numero_documento:
                            numeroDocumento,
                    };

                /*
                 * ENDPOINT 2:
                 * /api/credito/crearDocumentoSolicCredito
                 */
                const respuestaCreacion =
                    await api.postCreditCreateDocument(
                        payloadDocumento,
                    );

                if (
                    !respuestaCreacion?.isSuccess
                ) {
                    await NotificationService.genericAlert(
                        'Error',
                        respuestaCreacion?.returnMessage ||
                            'El archivo fue cargado, pero no se pudo registrar el documento.',
                        'error',
                    );

                    return;
                }

                const nuevoDocumento: DocumentoTemporal =
                    {
                        idTipoDocumento:
                            Number(id),

                        archivo,

                        rutaImagen,

                        creadoPor:
                            user.USR_ID,

                        fechaCreacion,

                        numero_documento:
                            numeroDocumento,
                    };

                setDocumentosTemporales(
                    documentosActuales => {
                        const documentosSinActual =
                            documentosActuales.filter(
                                documento =>
                                    documento.idTipoDocumento !==
                                    Number(
                                        id,
                                    ),
                            );

                        return [
                            ...documentosSinActual,
                            nuevoDocumento,
                        ];
                    },
                );

                setFilesUploaded(
                    archivosActuales => {
                        if (
                            archivosActuales.includes(
                                Number(id),
                            )
                        ) {
                            return archivosActuales;
                        }

                        return [
                            ...archivosActuales,
                            Number(id),
                        ];
                    },
                );

                await NotificationService.genericAlert(
                    'Archivo subido',
                    'El documento fue cargado correctamente.',
                );
            } catch {
                await NotificationService.genericAlert(
                    'Error',
                    'No fue posible cargar el documento.',
                    'error',
                );
            } finally {
                setIsLoadingFile(
                    false,
                );

                event.target.value =
                    '';
            }
        };

    const eliminarDocumentoTemporal = (
        idTipoDocumento: number,
    ) => {
        /*
         * Aquí solamente se elimina del arreglo
         * pendiente de DODO.
         *
         * El archivo ya fue registrado en GA2.
         */
        setDocumentosTemporales(
            documentosActuales =>
                documentosActuales.filter(
                    documento =>
                        documento.idTipoDocumento !==
                        idTipoDocumento,
                ),
        );

        setFilesUploaded(
            archivosActuales =>
                archivosActuales.filter(
                    id =>
                        id !==
                        idTipoDocumento,
                ),
        );
    };

    const obtenerDocumentoTemporal = (
        idTipoDocumento: number,
    ) =>
        documentosTemporales.find(
            documento =>
                documento.idTipoDocumento ===
                idTipoDocumento,
        );

    const verDocumentoTemporal = (
        documentoTemporal:
            | DocumentoTemporal
            | undefined,
    ) => {
        if (!documentoTemporal) {
            return;
        }

        const fileURL =
            URL.createObjectURL(
                documentoTemporal.archivo,
            );

        window.open(
            fileURL,
            '_blank',
        );

        window.setTimeout(
            () => {
                URL.revokeObjectURL(
                    fileURL,
                );
            },
            1000,
        );
    };

    const actualizarEstado = async (
        event:
            React.ChangeEvent<HTMLInputElement>,
        file: any,
    ) => {
        if (!file.rutA_IMAGEN) {
            await NotificationService.genericAlert(
                'Error en la verificación',
                'Se debe cargar un documento antes de la verificación.',
                'error',
            );

            return;
        }

        try {
            setIsLoadingFile(
                true,
            );

            const request = {
                dcS_APROBACION:
                    Number(
                        event.target.checked,
                    ),

                dcS_RUTA_IMAGEN:
                    file.rutA_IMAGEN,

                soC_ID:
                    procedureRequest.requestID,
            };

            const response =
                await api.postActualizaDocumentoLegalizacion(
                    request,
                );

            if (
                response?.isSuccess
            ) {
                await listarArchivos();
            }
        } catch {
            await NotificationService.genericAlert(
                'Error',
                'No se pudo actualizar el archivo. Intente nuevamente',
                'error',
            );
        } finally {
            setIsLoadingFile(
                false,
            );
        }
    };

    const renderTablaDocumentos = (
        titulo: string,
        documentos: any[],
    ) => {
        if (
            documentos.length ===
            0
        ) {
            return null;
        }

        return (
            <CHPaper
                title={
                    titulo
                }
            >
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell
                                component="th"
                                style={{
                                    width:
                                        '50%',
                                }}
                            >
                                Documento
                            </TableCell>

                            <TableCell
                                component="th"
                                style={{
                                    width:
                                        '10%',
                                    textAlign:
                                        'center',
                                }}
                            >
                                Obligatorio
                            </TableCell>

                            <TableCell
                                component="th"
                                style={{
                                    width:
                                        '25%',
                                    textAlign:
                                        'center',
                                }}
                            >
                                Archivo
                            </TableCell>

                            <TableCell
                                component="th"
                                style={{
                                    width:
                                        '15%',
                                    textAlign:
                                        'center',
                                }}
                            >
                                Acción
                            </TableCell>

                            {procedureRequest.flowActivityID ===
                                16 && (
                                <TableCell
                                    component="th"
                                    style={{
                                        textAlign:
                                            'center',
                                    }}
                                >
                                    Aprobación
                                </TableCell>
                            )}
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {documentos.map(
                            (
                                file,
                                key,
                            ) => {
                                const documentoTemporal =
                                    obtenerDocumentoTemporal(
                                        Number(
                                            file.tdC_ID,
                                        ),
                                    );

                                return (
                                    <TableRow
                                        key={`file-${file.tdC_ID}-${key}`}
                                    >
                                        <TableCell>
                                            {
                                                file.tipO_DOCUMENTO
                                            }
                                        </TableCell>

                                        <TableCell
                                            style={{
                                                textAlign:
                                                    'center',
                                            }}
                                        >
                                            {file.obligatorio ===
                                            'S'
                                                ? 'SI'
                                                : 'NO'}
                                        </TableCell>

                                        <TableCell
                                            style={{
                                                textAlign:
                                                    'center',
                                            }}
                                        >
                                            <CHFileUploader
                                                file={
                                                    file
                                                }
                                                filesUploaded={
                                                    filesUploaded
                                                }
                                                id={
                                                    file.tdC_ID
                                                }
                                                onChangeInput={
                                                    uploadFile
                                                }
                                            />

                                            {documentoTemporal && (
                                                <div
                                                    style={{
                                                        marginTop:
                                                            '6px',

                                                        fontSize:
                                                            '12px',

                                                        maxWidth:
                                                            '220px',

                                                        overflow:
                                                            'hidden',

                                                        textOverflow:
                                                            'ellipsis',

                                                        whiteSpace:
                                                            'nowrap',

                                                        marginLeft:
                                                            'auto',

                                                        marginRight:
                                                            'auto',
                                                    }}
                                                    title={
                                                        documentoTemporal
                                                            .archivo
                                                            .name
                                                    }
                                                >
                                                    {
                                                        documentoTemporal
                                                            .archivo
                                                            .name
                                                    }
                                                </div>
                                            )}
                                        </TableCell>

                                        <TableCell
                                            style={{
                                                textAlign:
                                                    'center',

                                                whiteSpace:
                                                    'nowrap',
                                            }}
                                        >
                                            {documentoTemporal ? (
                                                <>
                                                    <IconButton
                                                        size="small"
                                                        title="Ver archivo"
                                                        onClick={() =>
                                                            verDocumentoTemporal(
                                                                documentoTemporal,
                                                            )
                                                        }
                                                    >
                                                        <Visibility />
                                                    </IconButton>

                                                    <IconButton
                                                        size="small"
                                                        title="Quitar del envío a DODO"
                                                        onClick={() =>
                                                            eliminarDocumentoTemporal(
                                                                Number(
                                                                    file.tdC_ID,
                                                                ),
                                                            )
                                                        }
                                                    >
                                                        <DeleteOutline />
                                                    </IconButton>
                                                </>
                                            ) : (
                                                file.rutA_IMAGEN !==
                                                    null &&
                                                file.visualiza && (
                                                    <IconButton
                                                        size="small"
                                                        title="Ver archivo"
                                                        onClick={getFileS(
                                                            file.rutA_IMAGEN,
                                                        )}
                                                    >
                                                        <Visibility />
                                                    </IconButton>
                                                )
                                            )}
                                        </TableCell>

                                        {procedureRequest.flowActivityID ===
                                            16 && (
                                            <TableCell
                                                style={{
                                                    textAlign:
                                                        'center',
                                                }}
                                            >
                                                <CHCheckbox
                                                    label=" "
                                                    onChange={event =>
                                                        actualizarEstado(
                                                            event,
                                                            file,
                                                        )
                                                    }
                                                    checked={
                                                        file.dcS_APROBACION ===
                                                        1
                                                    }
                                                />
                                            </TableCell>
                                        )}
                                    </TableRow>
                                );
                            },
                        )}
                    </TableBody>
                </Table>
            </CHPaper>
        );
    };

    return (
        <>
            <LoadingComponent
                key="loader"
                loading={
                    isLoading ||
                    isLoadingFile
                }
            />

            {renderTablaDocumentos(
                'Documentos Afiliado',
                documentosAfiliado,
            )}

            {renderTablaDocumentos(
                'Documentos Seguros',
                documentosSeguros,
            )}

            {renderTablaDocumentos(
                'Documentos Vendedor',
                documentosVendedor,
            )}

            {renderTablaDocumentos(
                'Documentos Inmueble',
                documentosInmueble,
            )}
        </>
    );
}

const mapStateToProps = (
    state: AppState,
) => ({
    creditFlowDocumentsData:
        state.credit
            .creditFlowDocumentsData,

    creditBasicData:
        state.credit
            .creditBasicData,
});

const mapDispatchToProps = {
    startPostCreditFlowDocuments,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(
    RequestInformationUploadFiles,
);