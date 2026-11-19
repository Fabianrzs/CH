import React from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import ApiReportes from '../../../services/api-reportes.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

const GetFormato416Excel = async (
    item: any,
    texto: any,
    publicIpAddress: string | null,
    setIsLoadingFile: (value: boolean) => void
) => {

    const apiReportes = ApiReportes();
    const fecha = moment(new Date(item?.fechaAplicacion)).format('DD/MM/YYYY');
    const arrayReporte: any[] = [];

    const espacio = Object.fromEntries(
        Array.from({ length: 11 }, (_, i) => [`columna${i + 1}`, ""])
    );

    const subtitulo = {
        ...Object.fromEntries(
            Array.from({ length: 11 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "FORMATO 416",
    };

    arrayReporte.push(subtitulo);

    const subtituloCircular = {
        ...Object.fromEntries(
            Array.from({ length: 11 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "CIRCULAR EXTERNA  010  DE 2022",
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
        columna8: "FECHA DE CORTE",
        columna9: `${fecha}`,
        columna10: "",
        columna11: "",
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
        columna9: "DD/MM/AAAA",
        columna10: "",
        columna11: "",
    }

    arrayReporte.push(infoEntidad);
    arrayReporte.push(espacio);

    const titulosColumnas = {
        columna1: "Subcuenta",
        columna2: "Tipo de solicitud",
        columna3: "Estado de la solicitud",
        columna4: "Causal de rechazo",
        columna5: "Modalidad de crédito",
        columna6: "Código del departamento",
        columna7: "Código CIIU",
        columna8: "Monto de solicitudes",
        columna9: "Número de solicitudes",
        columna10: "",
        columna11: "Unidad de captura",
    }
    arrayReporte.push(titulosColumnas);

    const subtitulo2 = {
        ...Object.fromEntries(
            Array.from({ length: 11 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "Solicitudes aprobadas y rechazadas",
    };

    arrayReporte.push(subtitulo2);

    const lineas = texto
        .split(/\r?\n/)
        .filter((linea: string) => linea.trim() !== '');

    const registros = lineas.map((linea: string) => {
        let columna = '';
        let valor: number | string = 0;
        let subcuenta = '';

        // Identificar líneas que contienen 416
        if (linea.includes('416')) {

            // Obtener columna
            columna = linea.substring(12, 14);

            // Obtener número de registro
            const numeroRegistro = linea.substring(16, 22);

            // Convertir:
            // 000001 -> 001
            // 000002 -> 002....
            const numeroSubcuenta = Number(numeroRegistro);

            subcuenta = String(numeroSubcuenta).padStart(3, '0');

            // Obtener valor después de +
            const indiceMas = linea.indexOf('+');

            if (indiceMas !== -1) {
                const valorTexto = linea
                    .substring(indiceMas + 1)
                    .trim();

                // Quitar ceros iniciales
                const valorSinCeros = valorTexto.replace(/^0+/, '');

                // Si después del + todos son ceros
                if (valorSinCeros === '') {
                    valor = 0;
                } else {
                    valor = Number(valorSinCeros);
                }
            }
        }

        return {
            linea,
            subcuenta,
            columna,
            valor
        };
    });

    // Ordenar datos en array por columna para mostrar en excel
    const resultado = Object.values(
        registros.reduce((grupos: Record<string, any>, item: any) => {
            const { subcuenta, columna, valor } = item;

            // Ignorar líneas que no correspondan a registros 416
            if (!subcuenta || !columna) {
                return grupos;
            }

            // Crear automáticamente la fila de la subcuenta
            if (!grupos[subcuenta]) {
                grupos[subcuenta] = {
                    columna1: `\u200B${subcuenta}`,
                    columna2: '',
                    columna3: '',
                    columna4: '',
                    columna5: '',
                    columna6: '',
                    columna7: '',
                    columna8: '',
                    columna9: '',
                    columna10: '',
                    columna11: '\u200B01'
                };
            }

            // Columna del txt: 01, 02, 03...08
            const numeroColumna = Number(columna);

            // txt 01 -> columna2
            // txt 02 -> columna3...
            if (numeroColumna >= 1 && numeroColumna <= 8) {
                const columnaResultado = `columna${numeroColumna + 1}`;

                grupos[subcuenta][columnaResultado] = numeroColumna === 7? `\u200B${UtilService.formatCurrencyCuatro(valor)}` : valor;
            }

            return grupos;
        }, {})
    );

    arrayReporte.push(...resultado);
    arrayReporte.push(espacio);

    const columnasFinal = {
        columna1: "Columnas",
        columna2: "1",
        columna3: "2",
        columna4: "3",
        columna5: "4",
        columna6: "5",
        columna7: "6",
        columna8: "7",
        columna9: "8",
        columna10: "",
        columna11: "",
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
        ]
    }));

    const request = {
        columnDataList: columnDataList.map(item => ({
            data: item.data
        })),
        columnName: [
            "REPORTE SOLICITUDES DE ACCESO AL CRÉDITO", "", "", "", "", "", "", "", "", "", "",
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
        link.download = `Informe 416:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
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
            descripcion: 'Descarga de archivo Excel reporte informe 416'

        } as Auditoria;
        auditoriaApi.InsertAuditoria(requestAuditoria);
    } catch (error: any) {

        const descripcionError = `Error al generar reporte informe 536.
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
        toast('Error al generar reporte informe 416', { type: toast.TYPE.ERROR });
    }

}

export default GetFormato416Excel;