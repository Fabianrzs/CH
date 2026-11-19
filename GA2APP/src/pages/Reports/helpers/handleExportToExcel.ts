import moment from "moment";
import { toast } from "react-toastify";
import { ColumnFormatType } from "../../../enums/ColumnFormatType";
import ApiReportes from "../../../services/api-reportes.service";
import AuthService from "../../../services/auth.service";
import DownloadBlob from '../../../utils/blob';
import { ColumnData } from "../../productClient/components/reportes/types/ColumnsData.type";
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from "../../../types/Auditoria.model.type";
import UtilService from "../../../utils/util.service";

const formatFechaExcel = (value: any): string => {
    if (!value) return '';

    const parseDate = moment.parseZone(value);

    return parseDate.isValid()
        ? parseDate.format('DD-MM-YYYY')
        : '';
};

const formatDecimalExcel = (value: any): string => {
    if (value === null || value === undefined || value === '') return '';

    const numero = Number(value);

    if (Number.isNaN(numero)) return String(value);

    return numero.toLocaleString('es-CO', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 6
    });
};

const formatMonedaExcel = (value: any): string => {
    if (value === null || value === undefined || value === '') return '';

    const numero = Number(value);

    if (Number.isNaN(numero)) return String(value);

    return numero.toLocaleString('es-CO', {
        style: 'currency',
        currency: 'COP',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
};

const formatPorcentajeExcel = (value: any): string => {
    if (value === null || value === undefined || value === '') return '';

    const numero = Number(value);

    if (Number.isNaN(numero)) return String(value);

    return `${numero.toLocaleString('es-CO', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 6
    })}%`;
};

const handleExportToExcel = async (
    data: any[],
    columns: ColumnData[],
    filename: string,
    publicIpAddress: string | null
) => {
    const apiReportes = ApiReportes();

    const columnasDescargables = columns.filter(x => x.download ?? true);

    const columnDataList = data.map((row: any) => ({
        data: columnasDescargables.map(x => {
            const value = row[x.name];

            if (x.formatType === ColumnFormatType.Fecha) {
                return formatFechaExcel(value);
            }

            if (x.formatType === ColumnFormatType.Moneda) {
                return formatMonedaExcel(value);
            }

            if (x.formatType === ColumnFormatType.Porcentaje) {

                const numero = Number(row[x.name]);
            
                if (Number.isNaN(numero)) return '';
            
                return numero.toString();
            }

            if (x.formatType === ColumnFormatType.ValorDecimal) {
                return formatDecimalExcel(value);
            }

            if (x.formatType === ColumnFormatType.Estado) {
                return 'EN MORA';
            }
          

            return value?.toString() ?? '';
        })
    }));

    const request = {
        columnData: columnDataList.map(item => item.data),
        columnsInfo: columnasDescargables.map(x => ({
            name: x.label,
            formatType: x.formatType
        }))
    };

    const user = AuthService.getUserInfo();
    const auditoriaApi = AuditoriaApi();

    const nav = window.navigator as any;

    const usrAg =
        nav.userAgentData?.brands?.find(
            (x: { brand: string }) =>
                x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand || navigator.userAgent;

    try {
        const response = await apiReportes.GenerarExcelInvariantCulture(request);

        const blob = new Blob([response], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        });

        DownloadBlob(blob, filename);

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData?.platform || navigator.platform,
            nombreUsuario: user?.USR_USERNAME,
            usurioId: user?.USR_ID,
            ip: publicIpAddress,
            modulo: 'Consulta de cartera',
            hostname: document.location.host,
            tipoAccion: 'DESCARGAS',
            idTipoAccion: 5,
            descripcion: `Descarga de reporte en formato excel:
                         \n archivo: ${filename}`
        } as Auditoria;

        await auditoriaApi.InsertAuditoria(requestAuditoria);

    } catch (error: any) {
        const descripcionError = `Error al descargar Excel.
        - Error: ${error?.message || 'Error desconocido'}
        - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
        - Código HTTP: ${error?.response?.status || 'No disponible'}`;

        const requestAuditoriaError: Auditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData?.platform || navigator.platform,
            nombreUsuario: user?.USR_USERNAME,
            usurioId: user?.USR_ID,
            ip: publicIpAddress,
            modulo: 'Consulta de cartera',
            hostname: document.location.host,
            tipoAccion: 'ERRORES Y EXCEPCIONES',
            idTipoAccion: 8,
            descripcion: descripcionError
        };

        await auditoriaApi.InsertAuditoria(requestAuditoriaError);

        toast('Error al descargar archivo', { type: 'error' });
    }
};

export default handleExportToExcel;