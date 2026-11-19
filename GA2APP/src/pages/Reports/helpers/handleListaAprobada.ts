import { useEffect, useState } from "react";
import moment from "moment";
import { toast } from 'react-toastify';
import { ColumnFormatType } from "../../../enums/ColumnFormatType";
import ApiReportes from "../../../services/api-reportes.service";
import AuthService from "../../../services/auth.service";
import DownloadBlob from '../../../utils/blob';
import { ColumnData } from "../../productClient/components/reportes/types/ColumnsData.type";
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from "../../../types/Auditoria.model.type";

const handleListaAprobada = async (data: any[], columns: ColumnData[], filename: string, publicIpAddress: string | null) => {

    const apiReportes = ApiReportes();

    const columnDataList = data.map((row: any) => ({
        data: columns.filter(x => (x.download ?? true)).map(x => {

            if (x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                const parseDate = moment(row[x.name]);
                return parseDate.isValid()
                    ? parseDate.format("YYYY-MM-DD")
                    : '';
            }

            return row[x.name]?.toString() ?? ''
        })
    }));

    if (columnDataList) {

        const request = {
            columnData: columnDataList.map(item => item.data),
            columnsInfo: columns.filter(x => (x.download ?? true)).map(x => ({
                name: x.label,
                formatType: x.formatType
            }))
        };

        const response = await apiReportes.GenerarExcelInvariantCulture(request);
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
            modulo: 'Generar Listas',
            hostname: document.location.host
        } as Auditoria;
        requestAuditoria.tipoAccion = 'DESCARGAS';
        requestAuditoria.idTipoAccion = 5;
        requestAuditoria.descripcion = `Descarga de Lista Aprobada:
        \n archivo: ${filename}`;
        await auditoriaApi.InsertAuditoria(requestAuditoria);
    }
}

export default handleListaAprobada;