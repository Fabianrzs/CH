export interface Address {
    idTipoCalle: number;
    tipoCalleValue: string | undefined;
    numeroCalle1: number;
    idLetra1: number;
    direccionLetra1Value: string | undefined;
    direccionBis1: number;
    direccionBis1Value: string | undefined;
    idCardinal1: number;
    direccionCardinal1Value: string | undefined;
    numeroCalle2: number;
    idLetra2: number;
    direccionLetra2Value: string | undefined;
    direccionBis2: number;
    direccionBis2Value: string | undefined;
    idCardinal2: number;
    direccionCardinal2Value: string | undefined;
    numeroCalle3: number;
    idLetra3: number;
    direccionLetra3Value: string | undefined;
    idCardinal3: number;
    direccionCardinal3Value: string | undefined;
    direccionComplemento: string | undefined;
}

export const AddressInitial: Address = {
    idTipoCalle: 0,
    tipoCalleValue: '',
    numeroCalle1: 0,
    idLetra1: 0,
    direccionLetra1Value: '',
    direccionBis1: 0,
    direccionBis1Value: '',
    idCardinal1: 0,
    direccionCardinal1Value: '',
    numeroCalle2: 0,
    idLetra2: 0,
    direccionLetra2Value: '',
    direccionBis2: 0,
    direccionBis2Value: '',
    idCardinal2: 0,
    direccionCardinal2Value: '',
    numeroCalle3: 0,
    idLetra3: 0,
    direccionLetra3Value: '',
    idCardinal3: 0,
    direccionCardinal3Value: '',
    direccionComplemento: '',
};

export interface DireccionTipo {
    id: number;
    descripcion: string;
}

export interface DireccionCardinal {
    id: number;
    descripcion: string;
}

export interface DireccionAbecedario {
    id: number;
    descripcion: string;
}

export interface DireccionBis {
    id: number;
    descripcion: string;
}
