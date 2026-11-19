import moment from 'moment';
import {
    DATE_ALTERNATIVE_FORMAT,
    DATE_FORMAT,
    FULL_DATE_FORMAT,
    FULL_DATE_FORMAT_COMPLETE,
    FULL_DATE_TIME_FORMAT_COMPLETE,
    TIME_FORMAT,
} from '../constants/constants';
import HelperService from './helper.service';

class UtilService {
    static getDate(dateString: moment.MomentInput): string {        
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format(DATE_FORMAT);
        }
        return '';
    }

    static getDateCustom(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format("YYYY-MM-DD");
        }
        return '';
    }

    static getDateFormat(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format("DD/MM/YYYY");
        }
        return '';
    }

    static getAlternativeDate(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format(DATE_ALTERNATIVE_FORMAT);
        }
        return '';
    }

    static getTime(dateString: any): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format(TIME_FORMAT);
        }
        return '';
    }

    static getTimeFromString(dateString: any): Date {
        const dateMoment = moment(dateString, TIME_FORMAT);
        if (dateMoment.isValid()) {
            return dateMoment.toDate();
        }
        return moment().toDate();
    }

    static getFullDate(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format(FULL_DATE_FORMAT);
        }
        return '';
    }

    static getFullDateNew(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format('DD/MM/YY HH:mm:ss');
        }
        return '';
    }

    static getFullDateWithTimeDirection(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format(FULL_DATE_FORMAT_COMPLETE);
        }
        return '';
    }

    static getFullDateWithTime(dateString: moment.MomentInput): string {
        const dateMoment = moment(dateString);
        if (dateMoment.isValid()) {
            return dateMoment.format(FULL_DATE_TIME_FORMAT_COMPLETE);
        }
        return '';
    }

    static isAllBackendResponsesOK(responses: any[]): boolean {
        let isSuccess = true;
        responses.forEach(response => {
            if (response.isSuccess == null) {
                isSuccess = false;
            }
        });
        return isSuccess;
    }

    static getFileExtension(path: string): string | undefined {
        const re = /(?:\.([^.]+))?$/;
        return re.exec(path)?.pop();
    }

    static getMimeTypeFromExtension = (ext: string) => {
        type ExtensionMimes = {
            [key: string]: string;
        };
        const extToMimes: ExtensionMimes = {
            jpeg: 'image/jpeg',
            jpg: 'image/jpg',
            png: 'image/png',
            pdf: 'application/pdf',
        };

        if (Object.prototype.hasOwnProperty.call(extToMimes, ext)) {
            return extToMimes[ext];
        }
        return false;
    };

    static cleanCurrencyValue(value: any): number {
        let valueFormatted = 0;
        if (HelperService.isString(value)) {
            let valueFormattedString = value.replaceAll(',', '');
            valueFormattedString = valueFormattedString.replaceAll('$', '');
            valueFormatted = +valueFormattedString;
        } else if (Number.isInteger(value)) {
            valueFormatted = value;
        }
        return valueFormatted;
    }

    static formatCurrency(value: number | bigint): string {
       
        const formatter = new Intl.NumberFormat('en-EN', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
        });

        return formatter.format(value); /* $2,500.00 */
    }
    
    static formatCurrencyTres(value: number | bigint): string {
        const formatter = new Intl.NumberFormat('en-EN', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });

        return formatter.format(value); /* $2,500.00 */
    }

    static formatCurrencyCuatro(value: number | bigint): string {
        const formatter = new Intl.NumberFormat('es-ES', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
    
        return `$${formatter.format(value)}`; /* $2.500,00 */
    }

    static formatNumberValueDecimal(value: number | bigint): string {
        const formatter = new Intl.NumberFormat('de-DE', {
          minimumFractionDigits: 0,
          maximumFractionDigits: 5, 
        });
      
        return formatter.format(value); // Ej: 987602.3681 => 987.602,3681
      }

    static formatPercent(value: number | bigint): string {
       
        const formatter = new Intl.NumberFormat('en-EN', {
            style: 'percent',
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
        });

        return formatter.format(value);
    }

    static formatPercent3(value: number | bigint): string {
       
        const formatter = new Intl.NumberFormat('en-EN', {
            style: 'percent',
            minimumFractionDigits: 0,
            maximumFractionDigits: 3,
        });

        return formatter.format(value);
    }

    static formatPercentComma(value: number | bigint): string {
        const num = Number(value);
      
        const formatter = new Intl.NumberFormat('es-ES', {
          minimumFractionDigits: 0,
          maximumFractionDigits: 2,
        });
      
        return `${formatter.format(num)}%`; /* 8.7 => 8,7% */
      }

    static formatCurrencyDos(value: number | bigint): string {

    const formatterDolar = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      });
      return formatterDolar.format(value); /* $2,500.00 */
    }
    
    static roundValue(value: any, precision = 0): any {
        let result = value;
        if (typeof result === 'number' && !Number.isNaN(result)) {
            result = parseFloat(result.toFixed(precision));
        }
        return result;
    }

    static roundValue1(value: any, precision = 0): any {
        let result = value;
        if (typeof result === 'number' && !Number.isNaN(result)) {
            result = parseFloat(result.toFixed(6));
        }
        return result;
    }

    static formatNumber(value: number | bigint): string {
        return new Intl.NumberFormat('es-CO', {
            style: 'decimal',
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
        }).format(Number(value)); /* 2.500 */
    }
}

export default UtilService;
