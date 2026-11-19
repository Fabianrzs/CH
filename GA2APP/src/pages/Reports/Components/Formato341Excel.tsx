import React from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import ApiReportes from '../../../services/api-reportes.service';

const GetFormato341Excel = async(
    item: any,
    datosInfo: any,
    setIsLoadingFile: (value: boolean) => void
) => {

    const apiReportes = ApiReportes();
    const fecha = moment(new Date(item?.fechaAplicacion)).format('DD/MM/YYYY');
    const arrayReporte: any[]= [];

    const espacio = Object.fromEntries(
        Array.from({ length: 44 }, (_, i) => [`columna${i + 1}`, ""])
    );

    const subtitulo = {
        ...Object.fromEntries(
            Array.from({ length: 44 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna1: "FORMATO 341",
    };

    arrayReporte.push(subtitulo);

    const subtituloCircular = {
        ...Object.fromEntries(
            Array.from({ length: 44 }, (_, i) => [`columna${i + 1}`, ""])
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

    const subtituloInfoDeudor = {
        columna1: "DEUDOR",
        columna2: "",
        columna3: "",
        columna4: "",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "",
        columna10: "",
        columna11: "",
        columna12: "",
        columna13: "",
        columna14: ""
    }
    arrayReporte.push(subtituloInfoDeudor);

    const infoDeudor = {
        columna1: "",
        columna2: "TIPO",
        columna3: "IDENTIFICACION",
        columna4: "CHEQUEO",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "NOMBRE",
        columna10: "",
        columna11: "CIIU",
        columna12: "",
        columna13: "",
        columna14: "NATURALEZA JURIDICA"
    }

    arrayReporte.push(infoDeudor);
    arrayReporte.push(espacio);

    const numColumnas = Object.fromEntries(
        Array.from(
            { length: 44 },
            (_, i) => {
                const columna = i + 1;
    
                return [
                    `columna${columna}`,
                    columna <= 2 || columna === 43 || columna === 44
                        ? ""
                        : `${columna - 2}`
                ];
            }
        )
    );
    arrayReporte.push(numColumnas);

    const subtitulosColumnas = {
        ...Object.fromEntries(
            Array.from({ length: 44 }, (_, i) => [`columna${i + 1}`, ""])
        ),
        columna20: "Cuota esperada",
        columna22: "Cuota recibida",
        columna37: "SARC",
    };

    arrayReporte.push(subtitulosColumnas);

    const subColumnas = {
        columna1: "",
        columna2: "",
        columna3: "No. de operaciones",
        columna4: "",
        columna5: "Capital",
        columna6: "",
        columna7: "Intereses",
        columna8: "",
        columna9: "Otros conceptos",
        columna10: "",
        columna11: "Eliminadas",
        columna12: "",
        columna13: "Provisiones Individuales",
        columna14: "",
        columna15: "Garantía",
        columna16: "",
        columna17: "",
        columna18: "", 
        columna19: "Capital ",
        columna20: "",
        columna21: "Intereses",
        columna22: "",
        columna23: "Capital",
        columna24: "",
        columna25: "Intereses",
        columna26: "",
        columna27: "Redescuento",
        columna28: "Fecha inicial del crédito",
        columna29: "Fecha vencimiento del crédito",
        columna30: "Tasa promedio del crédito E.A",
        columna31: "Dias de Mora",
        columna32: "Modelo",
        columna33: "Probabilidad incumplimiento",
        columna34: "Componente individual procíclico",
        columna35: "",
        columna36: "Componente individual contracíclico",
        columna37: "",
        columna38: "Exposición",
        columna39: "",
        columna40: "Tasa de descuento",
        columna41: "",
        columna42: "Perdida dado el incumplimiento",
        columna43: "",
        columna44: "Unidad de captura",
    }

    arrayReporte.push(subColumnas);

    const iniciales = {
        columna1: "Subcuenta",
        columna2: "Concepto",
        columna3: "M/L",
        columna4: "M/E",
        columna5: "M/L",
        columna6: "M/E",
        columna7: "M/L",
        columna8: "M/E",
        columna9: "M/L",
        columna10: "M/E",
        columna11: "M/L",
        columna12: "M/E",
        columna13: "M/L",
        columna14: "M/E",
        columna15: "Vr. M/L",
        columna16: "Vr. M/E",
        columna17: "Tipo",
        columna18: "F. último avaluo", 
        columna19: "M/L",
        columna20: "M/E",
        columna21: "M/L",
        columna22: "M/E",
        columna23: "M/L",
        columna24: "M/E",
        columna25: "M/L",
        columna26: "M/E",
        columna27: "",
        columna28: "",
        columna29: "",
        columna30: "",
        columna31: "",
        columna32: "",
        columna33: "",
        columna34: "M/L",
        columna35: "M/E",
        columna36: "M/L",
        columna37: "M/E",
        columna38: "M/L",
        columna39: "M/E",
        columna40: "M/L",
        columna41: "M/E",
        columna42: "",
        columna43: "",
        columna44: "",
    }

    arrayReporte.push(iniciales);
    arrayReporte.push(espacio);

    const subtitulo1 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Garantía ídonea" : "",
        ])
    );

    arrayReporte.push(subtitulo1);

    const subtitulo2 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? " Otras garantías" : "",
        ])
    );

    arrayReporte.push(subtitulo2);

    const subtitulo3 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Garantía ídonea - Tarjetas de crédito" : "",
        ])
    );

    arrayReporte.push(subtitulo3);

    const subtitulo4 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Garantía ídonea - Automoviles" : "",
        ])
    );

    arrayReporte.push(subtitulo4);

    const subtitulo5 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Garantía ídonea - Otros" : "",
        ])
    );

    arrayReporte.push(subtitulo5);

    const subtitulo6 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Otras garantías - Tarjetas de crédito" : "",
        ])
    );

    arrayReporte.push(subtitulo6);

    const subtitulo7 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Otras garantías - Automoviles" : "",
        ])
    );

    arrayReporte.push(subtitulo7);

    const subtitulo8 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Otras garantías - Otros" : "",
        ])
    );

    arrayReporte.push(subtitulo8);

    const subtitulo9 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "VIS" : "",
        ])
    );

    arrayReporte.push(subtitulo9);
    
    const subtitulo10 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "No VIS" : "",
        ])
    );

    arrayReporte.push(subtitulo10);

    const subtitulo11 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Garantía ídonea" : "",
        ])
    );

    arrayReporte.push(subtitulo11);

    const subtitulo12 = Object.fromEntries(
        Array.from({ length: 44 }, (_, index) => [
            `columna${index + 1}`,
            index === 0 ? "Otras garantías" : "",
        ])
    );

    arrayReporte.push(subtitulo12);

    const totales = {
        columna1: "",
        columna2: "Totales",
        columna3: "",
        columna4: "",
        columna5: "",
        columna6: "",
        columna7: "",
        columna8: "",
        columna9: "",
        columna10: "",
        columna11: "",
        columna12: "",
        columna13: "",
    }

    arrayReporte.push(totales);
    arrayReporte.push(espacio);

    const numColumnasFinal = Object.fromEntries(
        Array.from({ length: 44 }, (_, i) => {
            const columna = i + 1;
    
            if (columna === 1 || columna === 43 || columna === 44) {
                return [`columna${columna}`, ""];
            }
    
            if (columna === 2) {
                return [`columna${columna}`, "Columnas"];
            }
    
            return [`columna${columna}`, `${columna - 2}`];
        })
    );

    arrayReporte.push(numColumnasFinal);

    const columnDataList = arrayReporte?.map((row: any) => ({
        data: Array.from(
            { length: 44 },
            (_, index) => String(row?.[`columna${index + 1}`] ?? "")
        )
    }));
    
    const crearColumnasTituloReporte = (titulo: string, cantidadColumnas: number) => [
        titulo,
        ...Array(cantidadColumnas - 1).fill(" ")
    ];
    
    const request = {
        columnDataList: columnDataList.map(item => ({
            data: item.data
        })),
        columnName: crearColumnasTituloReporte(
            "INFORME INDIVIDUAL POR DEUDOR DE OPERACIONES ACTIVAS DE CREDITO",
            43
        )
    };

    try{
        const response = await apiReportes.GenerarExcels(request);
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Informe 341:${moment(item?.fechaAplicacion).format('YYYY-MM-DD')}.xlsx`;
        document.body.appendChild(link);
        link.click();
        setIsLoadingFile(false);
        window.URL.revokeObjectURL(url);
    }catch(error: any){
        setIsLoadingFile(false);
        toast('Error al generar reporte informe 341', { type: toast.TYPE.ERROR });
   }

}

export default GetFormato341Excel;