import { EmbargoBeneficiariosDto } from "../../../types/embargo.model.type";

const convertToInsertBeneficiary = (embargoBeneficiario: EmbargoBeneficiariosDto) => ({
    radicadoWorkManager: embargoBeneficiario.radicadoWorkManager,
    tipoDocumentoId: embargoBeneficiario.tipoDocumentoId,
    numeroIdentificacion: embargoBeneficiario.numeroIdentificacion,
    primerNombre: embargoBeneficiario.primerNombre,
    segundoNombre: embargoBeneficiario.segundoNombre,
    primerApellido: embargoBeneficiario.primerApellido,
    segundoApellido: embargoBeneficiario.segundoApellido
})

export {
    convertToInsertBeneficiary
}