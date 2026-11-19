import { ConceptoEmbargosDto } from "../../../types/embargo.model.type";
import GenerarGuid from "../../solicitudes/components/funciones/guid.funtions";

const guid = GenerarGuid();

const convertToConceptos = (idCuenta: number, idConcepto: number, conceptoDescripcion: string,saldoConcepto: number, numeroCuenta: number) => ({
    id: guid.newGuid(),
    valor: null,
    fechaInicio: null,
    fechaFin: null,
    indicadorCesantias: false,
    tipoRetencionId: '',
    tipoEmbargoId: '',
    cuentaId: idCuenta,
    conceptoId: idConcepto,
    numeroCuenta: numeroCuenta,
    conceptoDescripcion: conceptoDescripcion,
    saldo: saldoConcepto,
    insert: true,
    tipoRetencion: ''
} as ConceptoEmbargosDto)

export {
    convertToConceptos,
}