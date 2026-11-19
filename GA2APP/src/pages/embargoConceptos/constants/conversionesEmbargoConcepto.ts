import { EmbargoConceptoDto } from "../../../types/embargo.model.type";

const convertToInsertEmbargoConcepto = (embargoConcepto: EmbargoConceptoDto, conceptoId: number) => ({
    valor: embargoConcepto.valor,
    fechaInicio: embargoConcepto.fechaInicio,
    fechaFin: embargoConcepto.fechaFin,
    indicadorCesantias: embargoConcepto.indicadorCesantias,
    tipoRetencionId: embargoConcepto.tipoRetencionId,
    tipoEmbargoId: embargoConcepto.tipoEmbargoId,
    radicadoWorkManager: embargoConcepto.radicadoWorkManager,
    cuentaId: embargoConcepto.cuentaId,
    conceptoId: conceptoId
})

export {
    convertToInsertEmbargoConcepto
}