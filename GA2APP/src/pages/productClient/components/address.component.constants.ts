import { Address } from '../../../types/address.type';

export const setFormDataAddres = (value: Address): string => {
    let result = '';

    if(value.tipoCalleValue === "" && value.numeroCalle1.toString() === "" && value.direccionLetra1Value === "" && value.direccionBis1Value === "" && value.direccionCardinal1Value === "" && value.numeroCalle2.toString() === "" && value.direccionLetra2Value === "" && value.direccionBis2Value === "" && value.direccionCardinal2Value ==="" && value.numeroCalle3.toString() === "" && value.direccionLetra3Value === "" && value.direccionCardinal3Value === "" && value.direccionComplemento !== "" ){
        result = `${value.direccionComplemento}`;
    }
    else if(value.tipoCalleValue === "" && value.numeroCalle1.toString() === "" && value.direccionLetra1Value === "" && value.direccionBis1Value === "" && value.direccionCardinal1Value === "" && value.numeroCalle2.toString() === "" && value.direccionLetra2Value === "" && value.direccionBis2Value === "" && value.direccionCardinal2Value ==="" && value.numeroCalle3.toString() === "" && value.direccionLetra3Value === "" && value.direccionCardinal3Value === "" && value.direccionComplemento === "" ){
        result = "";
    }
    else{
        result = `${`${value.tipoCalleValue} ${value.numeroCalle1}${value.direccionLetra1Value} ${value.direccionBis1Value} ${value.direccionCardinal1Value} `}# ${`${value.numeroCalle2}${value.direccionLetra2Value} ${value.direccionBis2Value} ${value.direccionCardinal2Value}`}- ${`${
            value.numeroCalle3
        }${value.direccionLetra3Value} ${value.direccionCardinal3Value} ${
            value.direccionComplemento != null ? value.direccionComplemento : ''
        }`}`;
    }
    return result;
};