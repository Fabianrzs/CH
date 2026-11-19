export const isNumeric = (val: string) => {
    const objRegExp = /^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$/;
    return objRegExp.test(val);
};
