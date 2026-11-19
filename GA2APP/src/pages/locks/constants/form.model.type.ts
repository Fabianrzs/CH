import { BloqueoDto } from '../../../types/lock.model.type';

export const LockForm: BloqueoDto = {
    id: 0,
    codigo: 0,
    concepto: '',
    diasMora: '',
    reversible: false,
    aceleracionDeuda: false,
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};
