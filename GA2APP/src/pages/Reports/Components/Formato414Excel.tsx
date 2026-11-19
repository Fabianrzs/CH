import React from 'react';
import moment, { defaultFormatUtc } from 'moment';
import { toast } from 'react-toastify';
import ApiReportes from '../../../services/api-reportes.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import UtilService from '../../../utils/util.service';

const GetFormato414Excel = async(
    item: any,
    datosInfo: any,
    publicIpAddress: string | null,
    setIsLoadingFile: (value: boolean) => void
) => { 

    const apiReportes = ApiReportes();
    const fecha = moment(item?.fechaAplicacion).format('DD/MM/YYYY');
    const arrayReporte: any[]= [];

    const espacio = Object.fromEntries(
        Array.from({ length: 14 }, (_, i) => [`columna${i + 1}`, ""])
    );

    const subtitulo = {
        ...Object.fromEntries(
            Array.from({ length: 14 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "FORMATO 414",
    };

    arrayReporte.push(subtitulo);

    const subtituloCircular = {
        ...Object.fromEntries(
            Array.from({ length: 44 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "CIRCULAR EXTERNA 034 DE 2021",
    };

    arrayReporte.push(subtituloCircular);
    arrayReporte.push(espacio);

    const subtituloFecha = {
        columna1: "",
        columna2: "",
        columna3: "",
        columna4: "",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "",
        columna10: "",
        columna11: "FECHA DE CORTE",
        columna12: "",
        columna13: "",
        columna14: ""
    }
    arrayReporte.push(subtituloFecha);

    const subtituloInfoEntidad = {
        columna1: "ENTIDAD",
        columna2: "22",
        columna3: "11",
        columna4: "IOECAJAPROMO",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "",
        columna10: "",
        columna11: `${fecha}`,
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
        columna10: "",
        columna11: "DD/MM/AAAA",
        columna12: "",
        columna13: "",
        columna14: ""
    }
    arrayReporte.push(infoEntidad);
    arrayReporte.push(espacio);

    const titulosColumnas = {
        columna1: "Subcuentas",
        columna2: "Tipo de persona",
        columna3: "Sexo",
        columna4: "Tamaño de empresa",
        columna5: "Tipo  de crédito",
        columna6: "Tipo de garantia",
        columna7: "Producto de crédito",
        columna8: "Plazo de crédito",
        columna9: "Tasa efectiva promedio ponderada",
        columna10: "Margen adicional a la variación para créditos en UVR",
        columna11: "Montos desembolsados",
        columna12: "Número de créditos desembolsados",
        columna13: "",
        columna14: "UNIDAD DE CAPTURA"
    }
    arrayReporte.push(titulosColumnas);

   
    if (datosInfo && datosInfo.length > 0) {
        const datos = datosInfo.map((item: any) => ({
            columna1: `\u200B${item?.subcuenta}`,
            columna2: item?.tipO_PERSONA,
            columna3: item?.sexo,
            columna4: item?.tamanO_EMPRESA,
            columna5: item?.tipO_CREDITO,
            columna6: item?.tipO_GARANTIA,
            columna7: item?.productO_CREDITO,
            columna8: item?.plazO_CREDITO,
            columna9: item?.tasA_EFECTIVA_PROMEDIO_PONDERADA,
            columna10: item?.margeN_ADICIONAL_VARIACIÓN_CREDITOS_UVR,
            columna11: `\u200B${UtilService.formatCurrencyCuatro(item?.montoS_DESEMBOLSADOS)}`,
            columna12: item?.numerO_CREDITOS_DESEMBOLSADOS,
            columna13: "",
            columna14: `\u200B${item?.unidaD_CAPTURA}`
        }));
    
        arrayReporte.push(...datos);
    }


    arrayReporte.push(espacio);
    const columnasFinal = {
        columna1: "COLUMNA",
        columna2: "1",
        columna3: "2",
        columna4: "3",
        columna5: "4",
        columna6: "5",
        columna7: "6",
        columna8: "7",
        columna9: "8",
        columna10: "9",
        columna11: "10",
        columna12: "11",
        columna13: "",
        columna14: ""
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
            String(row?.columna14),
        ]
    }));
    
    const request = {
        columnDataList: columnDataList.map(item => ({
            data: item.data
        })),
        columnName: [
            "TASA DE INTERÉS ACTIVAS POR TIPO DE CRÉDITO"," "," "," "," "," "," "," "," "," "," "," "," "," ",
        ]
    };

    const auditoriaApi = AuditoriaApi();
    const user = AuthService.getUserInfo();
    const nav = window.navigator as any;
    const usrAg = nav.userAgentData.brands.find(
        (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    )?.brand;

    try{
        const response = await apiReportes.GenerarExcels(request);
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        link.download = `Informe 414:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
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
            modulo: 'Generación de Reportes SfC',
            hostname: document.location.host,
            tipoAccion: 'DESCARGAS',
            idTipoAccion: 5,
            descripcion: 'Descarga de archivo Excel reporte informe 414'

        } as Auditoria;
        auditoriaApi.InsertAuditoria(requestAuditoria);
       
   }catch(error: any){
        const descripcionError = `Error al generar reporte informe 414.
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
        toast('Error al generar reporte informe 414', { type: toast.TYPE.ERROR });
   }
}

export default GetFormato414Excel;
