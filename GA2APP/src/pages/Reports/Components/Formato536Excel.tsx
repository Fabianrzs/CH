import React from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import ApiReportes from '../../../services/api-reportes.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

const generarRegistrosPorCaptura = (
    registros: any[],
    unidadCaptura: string
): any[] => {

    const columnasDinero = [
        '04',
        '05',
        '06',
        '07',
        '08',
        '09',
        '10',
        '11',
        '12',
        '13',
        '14',
        '15',
        '20',
        '22',
        '23',
        '27',
        '28'
    ];

    // Registros por cedula
    const registrosPorCedula: Record<string, any> = {};

    let consecutivo = 1;

    // Se filtra por unidad de captura
    registros
        .filter(
            (registro: any) =>
                registro?.numeroCaptura === unidadCaptura
        )
        .forEach((registro: any) => {

            const cedula = String(
                registro?.cedula ?? ''
            ).trim();

            const columna = String(
                registro?.columna ?? ''
            ).trim();

            const numeroCaptura = String(
                registro?.numeroCaptura ?? ''
            ).trim();

            if (!cedula || !columna) {
                return;
            }

            // Se crea registro por cedula
            if (!registrosPorCedula[cedula]) {

                registrosPorCedula[cedula] = {

                    // Consecutivo
                    columna1:
                        `\u200B${String(consecutivo).padStart(3, '0')}`,

                    // Columna 38 vacía
                    columna38: '',

                    // Unidad de captura
                    columna39:
                        `\u200B${numeroCaptura}`
                };

                consecutivo += 1;

                // De la columna 2 a la 37 se crea el registro
                for (let i = 2; i <= 37; i += 1) {

                    registrosPorCedula[cedula][
                        `columna${i}`
                    ] = 0;
                }
            }

            // Asignar a columna correspondiente
            const numeroColumna = Number(columna);

            const nombreColumna =
                `columna${numeroColumna + 1}`;

            const valor = registro?.valor ?? 0;

            // En la columna18 se agrega mascara fecha DD/MM/YYYY
            if (columna === '18') {

                const fechaTexto = String(
                    valor
                ).trim();

                if (
                    fechaTexto !== '' &&
                    fechaTexto !== '00000000'
                ) {

                    const anio =
                        fechaTexto.substring(0, 4);

                    const mes =
                        fechaTexto.substring(4, 6);

                    const dia =
                        fechaTexto.substring(6, 8);

                    registrosPorCedula[cedula][
                        nombreColumna
                    ] =
                        `\u200B${dia}/${mes}/${anio}`;

                } else {

                    registrosPorCedula[cedula][
                        nombreColumna
                    ] = 0;
                }

                // Columnas con la mascara de dinero
            } else if (
                columnasDinero.includes(columna)
            ) {

                registrosPorCedula[cedula][
                    nombreColumna
                ] =
                    `\u200B${UtilService.formatCurrencyCuatro(valor)}`;
                // Las otras columnas sin mascara
            } else {

                registrosPorCedula[cedula][
                    nombreColumna
                ] = valor;
            }
        });

    return Object.values(
        registrosPorCedula
    );
};

const GetFormato536Excel = async (
    item: any,
    texto: any,
    publicIpAddress: string | null,
    setIsLoadingFile: (value: boolean) => void
) => {

    const apiReportes = ApiReportes();
    const fecha = moment(new Date(item?.fechaAplicacion)).format('DD/MM/YYYY');
    const arrayReporte: any[] = [];

    const lineas = texto
        .split(/\r?\n/)
        .filter((linea: string) => linea.trim() !== '');

        const linea2 = lineas[1]?.trim() ?? '';

        const indiceN = linea2.indexOf('N');
        
        let cedula = '';
        let tipoDocumento = '';
        let nombre = '';
        
        if (indiceN !== -1) {
        
            const parteAntesDeN = linea2.substring(0, indiceN);
        
            const indiceCeros =
                parteAntesDeN.lastIndexOf('0000');
        
            if (indiceCeros !== -1) {
        
                // Cédula: después de los 4 ceros
                cedula = parteAntesDeN.substring(
                    indiceCeros + 4
                );
        
                // Tipo de documento: antes de los 4 ceros
                tipoDocumento = parteAntesDeN
                .substring(indiceCeros - 1, indiceCeros);
            }
        
            // Nombre: después de la N
            nombre = linea2
                .substring(indiceN + 1)
                .trim();
    }

    const espacio = Object.fromEntries(
        Array.from({ length: 39 }, (_, i) => [`columna${i + 1}`, ""])
    );

    const subtitulo = {
        ...Object.fromEntries(
            Array.from({ length: 39 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "FORMATO 536",
    };

    arrayReporte.push(subtitulo);

    const subtituloCircular = {
        ...Object.fromEntries(
            Array.from({ length: 39 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "CIRCULAR EXTERNA 002 DE 2018",
    };

    arrayReporte.push(subtituloCircular);
    arrayReporte.push(espacio);

    const subtituloInfoEntidad = Object.fromEntries(
        Array.from({ length: 39 }, (_, index) => [`columna${index + 1}`, ""])
    );

    Object.assign(subtituloInfoEntidad, {
        columna1: "ENTIDAD",
        columna2: "22",
        columna3: "11",
        columna4: "IOECAJAPROMO",
        columna32: "FECHA DE CORTE",
        columna33: `${fecha}`,
    });

    arrayReporte.push(subtituloInfoEntidad);
    arrayReporte.push(espacio);

    const infoEntidad = Object.fromEntries(
        Array.from({ length: 39 }, (_, index) => [`columna${index + 1}`, ""])
    );

    Object.assign(infoEntidad, {
        columna2: "TIPO",
        columna3: "CODIGO",
        columna4: "NOMBRE",
        columna33: "DD/MM/AAAA",
    });

    arrayReporte.push(infoEntidad);
    arrayReporte.push(espacio);

    const subtituloInfoDeudor = Object.fromEntries(
        Array.from({ length: 39 }, (_, index) => [`columna${index + 1}`, ""])
    );

    Object.assign(subtituloInfoDeudor, {
        columna1: "DEUDOR",
        columna2: `${tipoDocumento}`,
        columna3: `${cedula}`,
        columna4: "",
        columna5: `${nombre}`,
        columna6: "0000",
        columna7: "0",
    });

    arrayReporte.push(subtituloInfoDeudor);

    const infoDeudor = Object.fromEntries(
        Array.from({ length: 39 }, (_, index) => [`columna${index + 1}`, ""])
    );

    Object.assign(infoDeudor, {
        columna2: "TIPO",
        columna3: "IDENTIFICACION",
        columna4: "CHEQUEO",
        columna5: "NOMBRE",
        columna6: "CIIU",
        columna7: "NATURALEZA JURIDICA"
    });

    arrayReporte.push(infoDeudor);
    arrayReporte.push(espacio);

    const titulos1 = {
        columna1: "",
        columna2: "",
        columna3: "",
        columna4: "",
        columna5: "Saldo previo a la Modificación o Reestructuración",
        columna6: "",
        columna7: "Saldo resultado de la Modificación o Reestructuración",
        columna8: "",
        columna9: "Saldo a la Fecha de Corte",
        columna10: "",
        columna11: "Valor del pago en la fecha de corte",
        columna12: "",
        columna13: "",
        columna14: "Saldo de Provisiones (Capital, Intereses)",
        columna15: "",
        columna16: "",
        columna17: "",
        columna18: "",
        columna19: "",
        columna20: "",
        columna21: "",
        columna22: "",
        columna23: "PERÍODO DE GRACIA (MESES)",
        columna24: "",
        columna25: "CALIFICACION",
        columna26: "",
        columna27: "",
        columna28: "Númeror de Pagos Regulares y Efectivos",
        columna29: "",
        columna30: "",
        columna31: "TASA PREVIA A LA MODIFICACION O REESTRUCTURACION",
        columna32: "",
        columna33: "TASA PACTADA DESPUES DE LA MODIFICACION O  REESTRUCTURACION",
        columna34: "",
        columna35: "PLAZO (MESES)", columna36: "", columna37: "", columna38: "", columna39: "",
    }

    arrayReporte.push(titulos1);

    const titulos2 = {
        columna1: "Subcuenta",
        columna2: "Identificación del Crédito Modificado o Reestructurado",
        columna3: "Modalidad del Crédito",
        columna4: "Producto",
        columna5: "Capital",
        columna6: "Intereses",
        columna7: "Capital",
        columna8: "Intereses",
        columna9: "Capital",
        columna10: "Intereses",
        columna11: "Capital",
        columna12: "Intereses",
        columna13: "Valor de intereses causados con efecto en PyG en la fecha de corte",
        columna14: "Previa a la Modificación o Reestructuración",
        columna15: "En la Fecha de la Modificación o Reestructuración",
        columna16: "En la Fecha de Corte",
        columna17: "Días de Mora antes de la Modificación o Reestructuración",
        columna18: "Días de Mora a la Fecha de Corte",
        columna19: "Fecha de Formalización",
        columna20: "Tipo de Garantía",
        columna21: "Valor de la Garantía",
        columna22: "Número de Veces",
        columna23: "Capital",
        columna24: "Intereses",
        columna25: "Previa a la Modificación o Reestructuración",
        columna26: "En la Fecha de la Modificación o Reestructuración",
        columna27: "En la fecha de corte",
        columna28: "Capital",
        columna29: "Intereses",
        columna30: "Altura de Vida del Crédito al momento de la Modificación o Reestructuración",
        columna31: "Tipo",
        columna32: "Tasa-Spread",
        columna33: "Tipo",
        columna34: "Tasa-Spread",
        columna35: "Inicial",
        columna36: "Después de la Modificación o Reestructuración",
        columna37: "Tipo de Reestructuración",
        columna38: "",
        columna39: "UNIDAD DE CAPTURA"
    }

    arrayReporte.push(titulos2);

    let cedulaActual = '';

    const registros = lineas.map((linea: string) => {
        let cedula = cedulaActual;
        let columna = '';
        let numeroCaptura = '';
        let valor: number | string = 0;

        // Verificar líneas que contiene nombre
        const indiceN = linea.indexOf('N');

        if (indiceN !== -1) {
            const parteAntesDeN = linea.substring(0, indiceN);

            // Buscar los últimos 4 ceros antes de la N
            const indiceCeros = parteAntesDeN.lastIndexOf('0000');

            if (indiceCeros !== -1) {
                cedulaActual = parteAntesDeN.substring(
                    indiceCeros + 4
                );

                cedula = cedulaActual;
            }
        }

        // Detectar lineas que contienen 536
        if (linea.includes('536')) {
            // 536 + columna + captura
            columna = linea.substring(12, 14);
            numeroCaptura = linea.substring(14, 16);

            // El detalle pertenece a la última cédula encontrada
            cedula = cedulaActual;

            // Obtener valor despues de +
            const indiceMas = linea.indexOf('+');

            if (indiceMas !== -1) {
                const valorTexto = linea
                    .substring(indiceMas + 1)
                    .trim();

                // Quitar ceros iniciales
                const valorSinCeros = valorTexto.replace(/^0+/, '');

                // Si despues del + todos son ceros
                if (valorSinCeros === '') {
                    valor = 0;
                }
                // Si hay otros valores diferentes como 'A'
                else if (
                    Number.isNaN(Number(valorSinCeros))
                ) {
                    valor = valorSinCeros;
                }
                // Si hay un valor despues de ceros 00000000030088205
                else {
                    valor = Number(valorSinCeros);
                }
            }
        }

        return {
            linea,
            numeroCaptura,
            cedula,
            columna,
            valor
        };
    });

    const subtitulo1 = {
        ...Object.fromEntries(
            Array.from({ length: 39 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "Modificados",
    };

    arrayReporte.push(subtitulo1);

    const registrosUnidadCaptura1 = generarRegistrosPorCaptura(registros, '01');

    if(registrosUnidadCaptura1?.length > 0){
        arrayReporte.push(...registrosUnidadCaptura1);
    }
    else if(registrosUnidadCaptura1?.length === 0){
        arrayReporte.push(espacio);
    }

    const subtitulo2 = {
        ...Object.fromEntries(
            Array.from({ length: 39 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "Restructurados",
    };

    arrayReporte.push(subtitulo2);
    const registrosUnidadCaptura2 = generarRegistrosPorCaptura(registros, '02');
    if(registrosUnidadCaptura2?.length > 0){
        arrayReporte.push(...registrosUnidadCaptura1);
    }
    else if(registrosUnidadCaptura2?.length === 0){
        arrayReporte.push(espacio);
    }
    arrayReporte.push(espacio);

    const columnasFinal: Record<string, string> = Object.fromEntries(
        Array.from({ length: 39 }, (_, index) => [
            `columna${index + 1}`,
            ""
        ])
    );

    columnasFinal.columna1 = "COLUMNAS";

    for (let i = 1; i <= 36; i += 1) {
        columnasFinal[`columna${i + 1}`] = String(i);
    }

    arrayReporte.push(columnasFinal);

    const columnDataList = arrayReporte?.map((row: any) => ({
        data: Array.from(
            { length: 39 },
            (_, index) => String(row?.[`columna${index + 1}`] ?? '')
        )
    }));

    const request = {
        columnDataList: columnDataList.map(item => ({
            data: item.data
        })),

        columnName: [
            "INFORME INDIVIDUAL DE OPERACIONES ACTIVAS DE CREDITO MODIFICADAS Y REESTRUCTURADAS",
            ...Array(38).fill(" ")
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
        link.download = `Informe 536:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
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
            descripcion: 'Descarga de archivo Excel reporte informe 536'

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
        toast('Error al generar reporte informe 536', { type: toast.TYPE.ERROR });
    }

}
export default GetFormato536Excel;