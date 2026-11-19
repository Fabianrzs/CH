import { EstadoDto } from '../../../types/state.model.type';

export const StateForm: EstadoDto = {
    id: 0,
    codigo: 0,
    concepto: '',
    diasMoraActivaEstado: '',
    saldoDeuda: '',
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};
