import { Address } from '../../../../types/address.type';


export const setFormDataAddres = (value?: Address): string => {
    if (!value) return '';

    const safe = (v: any) =>
        v !== undefined && v !== null && String(v).trim() !== '' ? String(v).trim() : '';

    const partes = [
        safe(value.tipoCalleValue),
        safe(value.numeroCalle1) + safe(value.direccionLetra1Value),
        safe(value.direccionBis1Value),
        safe(value.direccionCardinal1Value),
        '#',
        safe(value.numeroCalle2) + safe(value.direccionLetra2Value),
        safe(value.direccionBis2Value),
        safe(value.direccionCardinal2Value),
        '-',
        safe(value.numeroCalle3) + safe(value.direccionLetra3Value),
        safe(value.direccionCardinal3Value),
        safe(value.direccionComplemento),
    ];

    return partes.filter(p => p !== '' && p !== '#' && p !== '-')
                  .reduce((acc, curr) => acc ? `${acc} ${curr}` : curr, '');
};
