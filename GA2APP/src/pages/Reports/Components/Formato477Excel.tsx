import React from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import ApiReportes from '../../../services/api-reportes.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

const GetFormato477Excel = async (
    item: any,
    datosInfo: any,
    publicIpAddress: string | null,
    setIsLoadingFile: (value: boolean) => void
) => {

    const apiReportes = ApiReportes();
    const fecha = moment(new Date(item?.fechaAplicacion)).format('DD/MM/YYYY');
    const arrayReporte: any[] = [];

    const espacio = Object.fromEntries(
        Array.from({ length: 13 }, (_, i) => [`columna${i + 1}`, ""])
    );

    const subtitulo = {
        ...Object.fromEntries(
            Array.from({ length: 13 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "FORMATO 477",
    };

    arrayReporte.push(subtitulo);

    const subtituloCircular = {
        ...Object.fromEntries(
            Array.from({ length: 13 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "CIRCULAR EXTERNA 016 DE 2015",
    };

    arrayReporte.push(subtituloCircular);
    arrayReporte.push(espacio);

    const subtituloInfoEntidad = {
        columna1: "ENTIDAD",
        columna2: "22",
        columna3: "11",
        columna4: "IOECAJAPROMO",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "FECHA DE CORTE",
        columna10: `${fecha}`,
        columna11: "",
        columna12: "",
        columna13: "",
        columna14: ""
    }
    arrayReporte.push(subtituloInfoEntidad);

    const infoEntidad = {
        columna1: "",
        columna2: "TIPO",
        columna3: "CODIGO",
        columna4: "NOMBRE",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "",
        columna10: "DD/MM/AAAA",
        columna11: "",
        columna12: "",
        columna13: "",
        columna14: ""
    }
    arrayReporte.push(infoEntidad);
    arrayReporte.push(espacio);

    const numColumnas = {
        columna1: "",
        columna2: "",
        columna3: "1",
        columna4: "2",
        columna5: "3",
        columna6: "4",
        columna7: "5",
        columna8: "6",
        columna9: "7",
        columna10: "8",
        columna11: "9",
        columna12: "",
        columna13: "",
        columna14: ""
    }

    arrayReporte.push(numColumnas);

    const subColumnas = {
        columna1: "",
        columna2: "",
        columna3: "",
        columna4: "Provisiones",
        columna5: "",
        columna6: "",
        columna7: "Gasto por provisiones",
        columna8: "",
        columna9: "",
        columna10: "Reintegro por provisiones",
        columna11: "",
        columna12: "",
        columna13: "",
        columna14: ""
    }

    arrayReporte.push(subColumnas);

    const titulosColumnas = {
        columna1: "Subcuenta",
        columna2: "Concepto",
        columna3: "Provisión Individual",
        columna4: "Componente Individual Procíclico",
        columna5: "Componente Individual Contracíclico",
        columna6: "Provisión Individual",
        columna7: "Componente Individual Procíclico",
        columna8: "Componente Individual Contracíclico",
        columna9: "Provisión Individual",
        columna10: "Componente Individual Procíclico",
        columna11: "Componente Individual Contracíclico",
        columna12: "",
        columna13: "Unidad de captura"
    }
    arrayReporte.push(titulosColumnas);

    const titulos: Record<string, string> = {
        '01': 'Cartera y Leasing Financiero Comercial',
        '02': 'Leasing Operativo Comercial',
        '03': 'Leasing Habitacional destinado a vivienda familiar',
        '04': 'Cartera y Leasing Financiero de Consumo',
        '05': 'Leasing Operativo Consumo',
        '06': 'Vivienda',
        '07': 'Cartera y Leasing Financiero de Microcrédito',
        '08': 'Leasing Operativo Microcrédito',
        '09': 'Castigos'
    };

    const subcuentasPorSeccion: Record<string, string[]> = {
        '01': ['010', '020', '030', '040', '050'],
        '02': ['010', '020', '030', '040', '050'],
        '03': ['010', '020', '030', '040', '050'],
        '04': ['010', '020', '030', '040', '050', '60', '70', '80', '90', '100'],
        '05': ['010', '020', '030', '040', '050'],
        '06': ['010', '020', '030', '040', '050'],
        '07': ['010', '020', '030', '040', '050'],
        '08': ['010', '020', '030', '040', '050'],
        '09': ['010', '020', '030', '040', '041', '050', '060', '070', '080']
    };

    const conceptosPorSeccion: Record<string, Record<string, string>> = {
        '01': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '02': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '03': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '04': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E',
            '60': 'Bajo monto A',
            '70': 'Bajo monto B',
            '80': 'Bajo monto C',
            '90': 'Bajo monto D',
            '100': 'Bajo monto E'
        },

        '05': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '06': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '07': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '08': {
            '010': 'A',
            '020': 'B',
            '030': 'C',
            '040': 'D',
            '050': 'E'
        },

        '09': {
            '010': 'Cartera y Leasing Financiero Comercial',
            '020': 'Leasing Operativo Comercial',
            '030': 'Leasing Habitacional destinado a vivienda familiar',
            '040': 'Cartera y Leasing Financiero de Consumo',
            '041': 'Cartera de Consumo de bajo monto',
            '050': 'Leasing Operativo Consumo',
            '060': 'Vivienda',
            '070': 'Cartera y Leasing Financiero de Microcrédito',
            '080': 'Leasing Operativo Microcrédito'
        }
    };

    if (datosInfo && datosInfo.length > 0) {

        for (let i = 1; i <= 9; i += 1) {

            // Esta es la unidad de captura - fila
            const unidadCaptura = String(i).padStart(2, '0');

            const datos: any[] = [];

            // Titulo de la seccion
            datos.push({
                columna1: titulos[unidadCaptura] ?? '',
                columna2: '',
                columna3: '',
                columna4: '',
                columna5: '',
                columna6: '',
                columna7: '',
                columna8: '',
                columna9: '',
                columna10: '',
                columna11: '',
                columna12: '',
                columna13: `\u200B${unidadCaptura}`
            });

            // Subcuentas esperadas para esta sección
            const subcuentasEsperadas =
                subcuentasPorSeccion[unidadCaptura] ?? [];

            // Conceptos de la sección
            const conceptos =
                conceptosPorSeccion[unidadCaptura] ?? {};

            // Recorrer las subcuentas
            subcuentasEsperadas.forEach((subcuenta) => {

                // Buscar todos los registros de esa subcuenta independientemente de la columna
                const itemsSubcuenta = datosInfo.filter(
                    (item: any) =>
                        String(item?.subcuenta ?? '').trim() ===
                        String(subcuenta).trim() &&
                        String(item?.unidadCaptura ?? '').trim() ===
                        unidadCaptura
                );

                const concepto = conceptos[subcuenta] ?? '';

                // Objeto que representa una fila
                const fila: any = {
                    columna1: `\u200B${subcuenta}`,
                    columna2: concepto,
                    columna3: '',
                    columna4: '',
                    columna5: '',
                    columna6: '',
                    columna7: '',
                    columna8: '',
                    columna9: '',
                    columna10: '',
                    columna11: '',
                    columna12: '',
                    columna13: `\u200B${unidadCaptura}`
                };

                // Recorrer los valores encontrados
                itemsSubcuenta.forEach((item: any) => {

                    const columnaItem = String(
                        item?.columna ?? ''
                    ).trim();

                    const valor = `\u200B${UtilService.formatCurrencyCuatro(
                        item?.valor
                    ) ?? ''}`;

                    // item.columna determina donde poner el valor
                    switch (columnaItem) {

                        case '01':
                            fila.columna3 = valor;
                            break;

                        case '02':
                            fila.columna4 = valor;
                            break;

                        case '03':
                            fila.columna5 = valor;
                            break;

                        case '04':
                            fila.columna6 = valor;
                            break;

                        case '05':
                            fila.columna7 = valor;
                            break;

                        case '06':
                            fila.columna8 = valor;
                            break;

                        case '07':
                            fila.columna9 = valor;
                            break;

                        case '08':
                            fila.columna10 = valor;
                            break;

                        case '09':
                            fila.columna11 = valor;
                            break;
                        default:
                            break;
                    }
                });

                datos.push(fila);
            });

            arrayReporte.push(...datos);
        }
    }

    arrayReporte.push(espacio);

    // Sumatoria totales
    const totalesPorColumna: Record<string, number> = {
        '01': 0,
        '02': 0,
        '03': 0,
        '04': 0,
        '05': 0,
        '06': 0,
        '07': 0,
        '08': 0,
        '09': 0
    };

    // Sumatoria totales por columna
    datosInfo?.forEach((item: any) => {
        const columna = String(item?.columna ?? '').trim();
        const numeroCaptura = Number(item?.unidadCaptura.replace(/^0/, '').replace(' ', ''));
        if (totalesPorColumna[columna] !== undefined) {

            const valor = Number(item?.valor ?? 0);

            if (Number.isFinite(valor) && numeroCaptura <= 9) {
                totalesPorColumna[columna] += valor;
            }
        }
    });

    const totales: any = {
        columna1: '',
        columna2: 'Totales',
        columna3: '',
        columna4: '',
        columna5: '',
        columna6: '',
        columna7: '',
        columna8: '',
        columna9: '',
        columna10: '',
        columna11: '',
        columna12: '',
        columna13: ''
    };

    // Mostrar totales en excel
    for (let i = 1; i <= 9; i += 1) {

        const columna = String(i).padStart(2, '0');
        const columnaExcel = `columna${i + 2}`;

        const total = totalesPorColumna[columna];

        if (total !== 0) {
            totales[columnaExcel] =
                `\u200B${UtilService.formatCurrencyCuatro(total)}`;
        }
    }

    arrayReporte.push(totales);
    arrayReporte.push(espacio);

    const columnasFinal = {
        columna1: "",
        columna2: "Columnas",
        columna3: "1",
        columna4: "2",
        columna5: "3",
        columna6: "4",
        columna7: "5",
        columna8: "6",
        columna9: "7",
        columna10: "8",
        columna11: "9",
        columna12: "",
        columna13: "",
    }
    arrayReporte.push(columnasFinal);

    const columnDataList = arrayReporte?.map((row: any) => ({
        data: [
            String(row?.columna1),
            String(row?.columna2),
            String(row?.columna3),
            String(row?.columna4),
            String(row?.columna5),
            String(row?.columna6),
            String(row?.columna7),
            String(row?.columna8),
            String(row?.columna9),
            String(row?.columna10),
            String(row?.columna11),
            String(row?.columna12),
            String(row?.columna13),
        ]
    }));

    const request = {
        columnDataList: columnDataList.map(item => ({
            data: item.data
        })),
        columnName: [
            "INFORME CONSOLIDADO DE PROVISIONES INDIVIDUALES DE CARTERA DE CRÉDITOS", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "
        ]
    };

    const auditoriaApi = AuditoriaApi();
    const user = AuthService.getUserInfo();
    const nav = window.navigator as any;
    const usrAg = nav.userAgentData.brands.find(
        (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    )?.brand;

    try {
        const response = await apiReportes.GenerarExcels(request);
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Informe 477:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
        document.body.appendChild(link);
        link.click();
        setIsLoadingFile(false);
        window.URL.revokeObjectURL(url);

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Generación de Reportes SFC',
            hostname: document.location.host,
            tipoAccion: 'DESCARGAS',
            idTipoAccion: 5,
            descripcion: 'Descarga de archivo Excel reporte informe 477'

        } as Auditoria;
        auditoriaApi.InsertAuditoria(requestAuditoria);
    } catch (error: any) {

        const descripcionError = `Error al generar reporte informe 477.
        - Error: ${error?.message || 'Error desconocido'}
        - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
        - Código HTTP: ${error?.response?.status || 'No disponible'}`;

        const requestAuditoriaError: Auditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData?.platform,
            nombreUsuario: user?.USR_USERNAME,
            usurioId: user?.USR_ID,
            ip: publicIpAddress,
            modulo: 'Generación de Reportes SFC',
            hostname: document.location.host,
            tipoAccion: 'ERRORES Y EXCEPCIONES',
            idTipoAccion: 8,
            descripcion: descripcionError
        };

        auditoriaApi.InsertAuditoria(requestAuditoriaError);

        setIsLoadingFile(false);
        toast('Error al generar reporte informe 477', { type: toast.TYPE.ERROR });
    }

}
export default GetFormato477Excel;