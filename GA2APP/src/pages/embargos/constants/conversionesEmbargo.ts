import {
    BeneficiarioDto, ConceptoEmbargosDto, EmbargoDto,
    EmbargoInsertDto,
    EmbargoModelDto,
    EmbargoUpdateDto
} from "../../../types/embargo.model.type";
import GenerarGuid from "../../solicitudes/components/funciones/guid.funtions";

const guid = GenerarGuid();

const convertToInsert = (embargo: EmbargoDto, idJuzgado: string) =>
({
    radicadoWorkManager: embargo.radicadoWorkManager,
    fechaRadicadoWorkManager: embargo.fechaRadicadoWorkManager,
    radicadoJuzgado: embargo.radicadoJuzgado,
    fechaRadicadoJuzgado: embargo.fechaRadicadoJuzgado,
    emailRespuesta: embargo.emailRespuesta,
    nombreDemandante: embargo.nombreDemandante,
    nombreDemandado: embargo.nombreDemandado,
    expedienteBancoAgrario: embargo.expedienteBancoAgrario,
    remanente: embargo.remanente,
    observaciones: embargo.observaciones,
    valor: embargo.valor,
    clienteIdentificacion: embargo.identificacionCliente,
    clienteTipoIdentificacion: embargo.clienteTipoIdentificacion,
    juzgadoId: idJuzgado,
    procesoId: embargo.procesoId,
    tipoEmbargoId: embargo.tipoEmbargoId,
} as EmbargoInsertDto);


const convertToUpdate = (embargo: EmbargoDto, idJuzgado: string) => (
    {
        id: embargo.id,
        radicadoWorkManager: embargo.radicadoWorkManager,
        fechaRadicadoWorkManager: embargo.fechaRadicadoWorkManager,
        radicadoJuzgado: embargo.radicadoJuzgado,
        fechaRadicadoJuzgado: embargo.fechaRadicadoJuzgado,
        emailRespuesta: embargo.emailRespuesta,
        nombreDemandante: embargo.nombreDemandante,
        nombreDemandado: embargo.nombreDemandado,
        expedienteBancoAgrario: embargo.expedienteBancoAgrario,
        remanente: embargo.remanente,
        observaciones: embargo.observaciones,
        valor: embargo.valor,
        clienteIdentificacion: embargo.identificacionCliente,
        clienteTipoIdentificacion: embargo.clienteTipoIdentificacion,
        juzgadoId: idJuzgado,
        procesoId: embargo.procesoId,
        tipoEmbargoId: embargo.tipoEmbargoId,
    } as EmbargoUpdateDto);

const convertEmbargoModelInsert = (data: EmbargoDto, beneficiariosPago: BeneficiarioDto[], conceptosEmbargos: ConceptoEmbargosDto[]) => ({
    id: guid.newGuid(),
    direccion: data.direccion,
    radicadoWorkManager: data.radicadoWorkManager,
    fechaRadicadoWorkManager: data.fechaRadicadoWorkManager,
    radicadoJuzgado: data.radicadoJuzgado,
    fechaRadicadoJuzgado: data.fechaRadicadoJuzgado,
    emailRespuesta: data.emailRespuesta,
    expedienteBancoAgrario: data.expedienteBancoAgrario,
    remanente: data.remanente,
    Inconsistencia: data.Inconsistencia,
    observaciones: data.observaciones,
    aclaracion: data.aclaracion,
    clienteTipoIdentificacion: Number(data.clienteTipoIdentificacion),
    clienteIdentificacion: data.identificacionCliente,
    juzgadoId: data.juzgadoId,
    procesoId: data.procesoId,
    proceso: data.proceso,
    beneficiariosPagos: beneficiariosPago,
    conceptosEmbargos: conceptosEmbargos,
} as EmbargoModelDto)

const convertEmbargoModelUpdate = (data: EmbargoDto, beneficiariosPago: BeneficiarioDto[], conceptosEmbargos: ConceptoEmbargosDto[]) => ({
    id: data.id,
    direccion: data.direccion,
    radicadoWorkManager: data.radicadoWorkManager,
    fechaRadicadoWorkManager: data.fechaRadicadoWorkManager,
    radicadoJuzgado: data.radicadoJuzgado,
    fechaRadicadoJuzgado: data.fechaRadicadoJuzgado,
    emailRespuesta: data.emailRespuesta,
    expedienteBancoAgrario: data.expedienteBancoAgrario,
    remanente: data.remanente,
    Inconsistencia: data.Inconsistencia,
    observaciones: data.observaciones,
    aclaracion: data.aclaracion,
    clienteTipoIdentificacion: Number(data.clienteTipoIdentificacion),
    clienteIdentificacion: data.identificacionCliente,
    juzgadoId: data.juzgadoId,
    procesoId: data.procesoId,
    proceso: data.proceso,
    beneficiariosPagos: beneficiariosPago,
    conceptosEmbargos: conceptosEmbargos,
} as EmbargoModelDto)

export {
    convertToInsert,
    convertToUpdate,
    convertEmbargoModelInsert,
    convertEmbargoModelUpdate
}