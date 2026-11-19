import moment from "moment";
import { ColumnFormatType } from "../../../enums/ColumnFormatType";
import ApiReportes from "../../../services/api-reportes.service";
import AuthService from "../../../services/auth.service";
import DownloadBlob from '../../../utils/blob';
import { ColumnData } from "../../productClient/components/reportes/types/ColumnsData.type";
import { ExcelSheet } from "../../productClient/components/reportes/types/ExcelReport.type";
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from "../../../types/Auditoria.model.type";

type DataSheet = {
    data: any[],
    columns: ColumnData[],
    sheetName: string
}

const exportToExcel = async (filename: string, sheets: DataSheet[], publicIpAddress: string | null) => {

    const apiReportes = ApiReportes();
    const response = await apiReportes.GenerarReporteExcel({
        filename: filename,
        sheets: sheets.map(s => getSheet(s))
    });

    const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    DownloadBlob(blob, filename);
    const user = AuthService.getUserInfo();
    const auditoriaApi = AuditoriaApi();
    const nav = (window.navigator as any);
    const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
    const requestAuditoria = {
        navegador: usrAg,
        sistemaOperativo: nav.userAgentData.platform,
        nombreUsuario: user.USR_USERNAME,
        usurioId: user.USR_ID,
        ip: publicIpAddress,
        modulo: 'Reporte capital corriente',
        hostname: document.location.host
    } as Auditoria;
    requestAuditoria.tipoAccion = 'DESCARGAS';
    requestAuditoria.idTipoAccion = 5;
    requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                                 \n archivo: ${filename}`;
    await auditoriaApi.InsertAuditoria(requestAuditoria);
}

const getSheet = (sheet: DataSheet): ExcelSheet => {
    const data = sheet.data.map((row: any) =>
        sheet.columns.filter(x => (x.download ?? true)).map(x => {
            if (x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                const parseDate = moment(row[x.name]);
                return parseDate.isValid()
                    ? parseDate.format("YYYY-MM-DD")
                    : '';
            }

            return row[x.name]?.toString() ?? ''
        })
    );

    return {
        sheetName: sheet.sheetName,
        columnData: data,
        columnsInfo: sheet.columns.filter(x => (x.download ?? true)).map(x => ({
            name: x.label,
            formatType: x.formatType
        }))
    };
};

export default exportToExcel;