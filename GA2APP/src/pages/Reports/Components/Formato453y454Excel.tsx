import React from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import ApiReportes from '../../../services/api-reportes.service';
import UtilService from '../../../utils/util.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';

const GetFormato453y454Excel = async(
    item: any,
    datosInfo: any,
    publicIpAddress: string | null,
    setIsLoadingFile: (value: boolean) => void
) => {

        const apiReportes = ApiReportes();

        // Reporte 453
        const fecha = moment(item?.fechaAplicacion).format('DD/MM/YYYY');
        const arrayReporte453: any[]= [];

        const espacio = Object.fromEntries(
            Array.from({ length: 18 }, (_, i) => [`columna${i + 1}`, ""])
        );

        const subtitulo = {
            ...Object.fromEntries(
                Array.from({ length: 18 }, (_, i) => [`columna${i + 1}`, ""])
            ),
            columna1: "FORMATO 453",
        };
    
        arrayReporte453.push(subtitulo);

        const subtituloCircular = {
            ...Object.fromEntries(
                Array.from({ length: 18 }, (_, i) => [`columna${i + 1}`, ""])
            ),
            columna1: "CIRCULAR EXTERNA 011 DE 2008",
        };

        arrayReporte453.push(subtituloCircular);
        arrayReporte453.push(espacio);

        const subtituloInfoEntidad = {
            columna1: "ENTIDAD",
            columna2: "22",
            columna3: "11",
            columna4: "",
            columna5: "IOECAJAPROMO",
            columna6: "",
            columna7: "",
            columna8: "",
            columna9: "",
            columna10: "",
            columna11: "",
            columna12: "",
            columna13: "",
            columna14: "FECHA DE CORTE",
            columna15: `${fecha}`,
            columna16: "",
            columna17: "",
            columna18: ""
        }
        arrayReporte453.push(subtituloInfoEntidad);

        const infoEntidad = {
            columna1: "",
            columna2: "TIPO",
            columna3: "CODIGO",
            columna4: "",
            columna5: "NOMBRE",
            columna6: "",
            columna7: "",
            columna8: "",
            columna9: "",
            columna10: "",
            columna11: "",
            columna12: "",
            columna13: "",
            columna14: "",
            columna15: "DD/MM/AAAA",
            columna16: "",
            columna17: "",
            columna18: ""
        }

        arrayReporte453.push(infoEntidad);
        arrayReporte453.push(espacio);

        const titulosColumnas = {
            columna1: "SUBCUENTAS",
            columna2: "Saldo de la cartera a la fecha de corte del reporte",
            columna3: "Vigente",
            columna4: "Vencida 1-2 Meses",
            columna5: "Vencida 2-3 Meses",
            columna6: "Vencida 1-3 Meses",
            columna7: "Vencida 3-4 Meses",
            columna8: "Vencida > de 4 Meses",
            columna9: "Vencida 3-6 Meses",
            columna10: "Vencida > de 6 meses",
            columna11: "Vencida 1-4 meses",
            columna12: "Vencida 4-6 meses",
            columna13: "Vencida 6-12 meses",
            columna14: "Vencida 12-18 meses",
            columna15: "Vencida > de 12 meses",
            columna16: "Vencida > de 18 meses",
            columna17: "",
            columna18: "UNIDAD DE CAPTURA",
        }
        arrayReporte453.push(titulosColumnas);


    if (datosInfo && datosInfo.length > 0) {

        const datosFiltrados = datosInfo.filter(
            (item: any) => item?.formato === '453'
        );

        if(datosFiltrados && datosFiltrados.length > 0){
            const datos = [];

            const titulos: Record<string, string> = {
                '01': 'Crédito Rotativo',
                '02': 'Tarjetas de Crédito',
                '03': 'Libre Inversión',
                '04': 'Libranza',
                '05': 'Vehículo',
                '06': 'Otros portafolios de consumo',
                '07': 'Microcréditos Menores o Iguales a 25 SMMLV',
                '08': 'Microcréditos Superiores a 25 SMMLV y hasta 120 SMMLV',
                '09': 'Vivienda VIS Pesos',
                '10': 'Vivienda VIS UVR',
                '11': 'Vivienda No VIS Pesos',
                '12': 'Vivienda No VIS UVR',
                '13': 'Leasing Habitacional VIS Pesos',
                '14': 'Leasing Habitacional VIS UVR',
                '15': 'Leasing Habitacional No VIS Pesos',
                '16': 'Leasing Habitacional No VIS UVR',
                '17': '',
                '18': ''
            };
    
            for (let i = 1; i <= 16; i += 1) {
    
                const columna = String(i).padStart(2, '0');
    
                const items = datosFiltrados.filter(
                    (item: any) => item?.columna2_formateada === columna
                );
    
                datos.push({
                    columna1: titulos[columna],
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
                    columna13: '',
                    columna14: '',
                    columna15: '',
                    columna16: '',
                    columna17: '',
                    columna18: ''
                });
    
                if (items.length === 0) {
    
                    datos.push({
                        columna1: '',
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
                        columna13: '',
                        columna14: '',
                        columna15: '',
                        columna16: '',
                        columna17: '',
                        columna18: `\u200B${columna}`
                    });
    
                } else {
    
                    items.forEach((item: any) => {
    
                        datos.push({
                            columna1: `\u200B${item?.subcuenta ?? ''}`,
                            columna2: item?.columnaTable === '01'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna3: item?.columnaTable === '02'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna4: item?.columnaTable === '03'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna5: item?.columnaTable === '04'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna6: item?.columnaTable === '05'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna7: item?.columnaTable === '06'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna8: item?.columnaTable === '07'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna9: item?.columnaTable === '08'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna10: item?.columnaTable === '09'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna11: item?.columnaTable === '10'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna12: item?.columnaTable === '11'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna13: item?.columnaTable === '12'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna14: item?.columnaTable === '13'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna15: item?.columnaTable === '14'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna16: item?.columnaTable === '15'
                            ? `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`
                                : '',
                            columna17: '',
                            columna18: `\u200B${columna}`
                        });
    
                    });
                }
            }
    
            arrayReporte453.push(...datos);
        }
    }
        arrayReporte453.push(espacio);

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
            columna13: "12",
            columna14: "13",
            columna15: "14",
            columna16: "15",
            columna17: "",
            columna18: "",
        }
        arrayReporte453.push(columnasFinal);

        const columnDataList = arrayReporte453?.map((row: any) => ({
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
                String(row?.columna15),
                String(row?.columna16),
                String(row?.columna17),
                String(row?.columna18),
            ]
        }));
        
        const request = {
            columnDataList: columnDataList.map(item => ({
                data: item.data
            })),
            columnName: [
                "DISTRIBUCION DE SALDO POR PRODUCTOS"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ",
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
            link.download = `Informe 453:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
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
                descripcion: 'Descarga de archivo Excel reporte informe 453'

            } as Auditoria;
            auditoriaApi.InsertAuditoria(requestAuditoria);
        }catch(error: any){

            const descripcionError = `Error al generar reporte informe 453.
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
            toast('Error al generar reporte informe 453', { type: toast.TYPE.ERROR });
        }


        // Reporte 454
        const arrayReporte454: any[]= [];

        const espacio454 = {
            columna1: "",
            columna2: "",
            columna3: "",
            columna4: "",
            columna5: "",
            columna6: "",
        }

        const subtitulo454 = {
            columna1: "FORMATO 454",
            columna2: "",
            columna3: "",
            columna4: "",
            columna5: "",
            columna6: "",
        }

        arrayReporte454.push(subtitulo454);

        const subtituloCircular454 = {
            columna1: "CIRCULAR EXTERNA  011 DE 2008",
            columna2: "",
            columna3: "",
            columna4: "",
            columna5: "",
            columna6: "",
        }

        arrayReporte454.push(subtituloCircular454);
        arrayReporte454.push(espacio454);

        const subtituloInfoEntidad454 = {
            columna1: "ENTIDAD",
            columna2: "22",
            columna3: "11",
            columna4: "IOECAJAPROMO",
            columna5: "FECHA DE CORTE",
            columna6: `${fecha}`,
        }

        arrayReporte454.push(subtituloInfoEntidad454);

        const infoEntidad454 = {
            columna1: "",
            columna2: "TIPO",
            columna3: "CODIGO",
            columna4: "NOMBRE",
            columna5: "",
            columna6: "DD/MM/AAAA",
        }
        arrayReporte454.push(infoEntidad454);
        arrayReporte454.push(espacio454);

        const titulosColumnas454 = {
            columna1: "Subcuenta",
            columna2: "Descripción",
            columna3: "Número de créditos aprobados o desembolsados",
            columna4: "Montos aprobados o desembolsados",
            columna5: "",
            columna6: "Unidad de Captura",
        }

        arrayReporte454.push(titulosColumnas454);

        if (datosInfo && datosInfo.length > 0) {

            const datosFiltrados = datosInfo.filter(
                (item: any) => item?.formato === '454'
            );

            const titulos: Record<string, string> = {
                '005': 'Crédito Rotativo',
                '010': 'Tarjetas de Crédito de clientes con ingresos hasta 2 SMMLV',
                '015': 'Tarjetas de Crédito de clientes con ingresos superiores a 2 SMMLV',
                '020': 'Libre Inversión',
                '025': 'Libranza',
                '030': 'Vehículo',
                '035': 'Compra de Cartera de Consumo',
                '040': 'Microcréditos Menores o Iguales a 25 SMMLV',
                '045': 'Microcréditos Superiores a 25 SMMLV y hasta 120 SMMLV',
                '050': 'Compra de Cartera de Microcréditos',
                '055': 'Vivienda VIS Pesos',
                '060': 'Vivienda VIS UVR',
                '065': 'Vivienda No VIS Pesos',
                '070': 'Vivienda No VIS UVR',
                '075': 'Leasing Habitacional VIS Pesos',
                '080': 'Leasing Habitacional VIS UVR',
                '085': 'Leasing Habitacional No VIS Pesos',
                '090': 'Leasing Habitacional No VIS UVR',
                '095': 'Compra Cartera Vivienda en Pesos',
                '100': 'Compra Cartera Vivienda en UVR'
            };

            if(datosFiltrados && datosFiltrados.length > 0){

                const datos: any[] = [];
                Object.entries(titulos).forEach(([subcuenta, titulo]) => {

                    const items = datosFiltrados.filter(
                        (item: any) => item?.subcuenta === subcuenta
                    );
    
                    if (items.length > 0) {
    
                        items.forEach((item: any) => {
                            datos.push({
                                columna1: `\u200B${subcuenta}`,
                                columna2: titulo,
                                columna3: '',
                                columna4: `\u200B${UtilService.formatCurrencyCuatro(item?.valor_formateado) ?? ''}`,
                                columna5: '',
                                columna6: `\u200B${item?.columna2_formateada}`,
                            });
                        });
    
                    } else {
    
                        datos.push({
                            columna1: `\u200B${subcuenta}`,
                            columna2: titulo,
                            columna3: '',
                            columna4: '',
                            columna5: '',
                            columna6: '',
                        });
                    }
                });
    
                arrayReporte454.push(...datos);
            }

        
        }

        const columnasFinal454 = {
            columna1: "COLUMNA",
            columna2: "1",
            columna3: "2",
            columna4: "",
            columna5: "",
            columna6: "",
        }
        arrayReporte454.push(espacio454);
        arrayReporte454.push(columnasFinal454);

        const columnDataList454 = arrayReporte454?.map((row: any) => ({
            data: [
                String(row?.columna1),
                String(row?.columna2),
                String(row?.columna3),
                String(row?.columna4),
                String(row?.columna5),
                String(row?.columna6),
            ]
        }));
        
        const request454 = {
            columnDataList: columnDataList454.map(item => ({
                data: item.data
            })),
            columnName: [
                "MONTOS Y NÚMERO DE CRÉDITOS APROBADOS O DESEMBOLSADOS POR COSECHAS","","","","","",
            ]
        };

        try{
            const response = await apiReportes.GenerarExcels(request454);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Informe 454:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
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
                descripcion: 'Descarga de archivo Excel reporte informe 453'

            } as Auditoria;
            auditoriaApi.InsertAuditoria(requestAuditoria);
        }catch(error: any){

            const descripcionError = `Error al generar reporte informe 453.
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
            toast('Error al generar reporte informe 454', { type: toast.TYPE.ERROR });
        }

}

export default GetFormato453y454Excel;
