import { ObtenerDesembargoDto, DesembargoDto } from "../../../types/desembargo.model.type"

const convertToInsertDesembargoConcepto = (desembargoConcepto: ObtenerDesembargoDto, conceptoId: number) => ({
    valor: desembargoConcepto.valor,
    tipoEmbargoId: desembargoConcepto.tipoEmbargoId,
    radicadoWorkManager: desembargoConcepto.radicadoWorkManager,
    clienteId: desembargoConcepto.clienteId,
    conceptoId: conceptoId
})

export {
    convertToInsertDesembargoConcepto
}