import _ from 'lodash';

class HelperService {
    static partial(fn: any, ...args: any): any {
        return _.partial(fn, args);
    }

    static omit(object: any, paths: string[]): any {
        return _.omit(object, paths);
    }

    static isString(value: any): boolean {
        return _.isString(value);
    }
}
export default HelperService;
