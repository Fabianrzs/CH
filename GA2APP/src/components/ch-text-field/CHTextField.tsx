import React, { useEffect, useState } from 'react';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { TextField, TextFieldProps } from '@material-ui/core';
import MaskedInput from 'react-text-mask';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';
import HelperService from '../../utils/helper.service';

const useStylesContainer = makeStyles(() =>
    createStyles({
        root: {
            width: '100%',
        },
    }),
);

const useStylesReddit = makeStyles(() =>
    createStyles({
        input: {
            fontSize: '.9rem',
            width: '100%',
        },
    }),
);

const useStylesLabel = makeStyles(() =>
    createStyles({
        root: {
            width: '100%',
            maxWidth: '100%',
            textAlign: 'left',
        },
    }),
);

type CHTextFieldProps = TextFieldProps & {
    isReadOnly?: boolean;
    currencyMask?: boolean;
    currencyMaskDecimal?: boolean;
    cellPhoneMask?: boolean;
    phoneMask?: boolean;
    percentMask?: boolean;
    decimalsMaskNumber?: boolean;
    decimalsMask?: boolean;
    decimalsMaskDos?: boolean;
    decimalsMaskByNumber?: boolean;
    numberMask?: boolean | null;
    numberOnlyMask?: boolean | null;
    numberMaskInt?: boolean | null;
    nameMask?: boolean;
    numberAniosMask?: boolean | null;
    numberMaskTres?: boolean | null;
    pesosEnterosMask?: boolean | null;
    currencyMaskOptions?: any;
};

const defaultNumberMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2,
    integerLimit: 15,
    allowNegative: false,
    allowLeadingZeroes: false,
};
const defaultNumberOnlyMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: true,
    decimalSymbol: ',',
    decimalLimit: 0, // how many digits allowed after the decimal
    integerLimit: 21, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: true,
};

const defaultNumberMaskIntOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2,
    integerLimit: 5,
    allowNegative: false,
    allowLeadingZeroes: false,
};
const defaultNumberAniosMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: true,
    thousandsSeparatorSymbol: ',',
    allowDecimal: true,
    decimalSymbol: '.',
    decimalLimit: 0,
    integerLimit: 2,
    allowNegative: false,
    allowLeadingZeroes: false,
}
const defaultnumbermakTresOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false, // Desactivar el separador de miles
    thousandsSeparatorSymbol: '',
    allowDecimal: false, // Desactivar los decimales
    decimalSymbol: '',
    decimalLimit: 0,
    integerLimit: 3,
    allowNegative: false,
    allowLeadingZeroes: true, // Permitir ceros iniciales
};

const defaultNumberIntMaskOptions = {
    prefix: '$',
    suffix: '',
    includeThousandsSeparator: true,
    thousandsSeparatorSymbol: ',',
    allowDecimal: true,
    decimalSymbol: '.',
    decimalLimit: 0,
    integerLimit: 15,
    allowNegative: false,
    allowLeadingZeroes: false,
}

const defaultMaskOptions = {
    prefix: '$',
    suffix: '',
    includeThousandsSeparator: true,
    thousandsSeparatorSymbol: ',',
    allowDecimal: true,
    decimalSymbol: '.',
    decimalLimit: 2,
    integerLimit: 15,
    allowNegative: false,
    allowLeadingZeroes: false,
};

const defaultMaskOptionsDecimal = {
    prefix: '$',
    suffix: '',
    includeThousandsSeparator: true,
    thousandsSeparatorSymbol: '.',
    allowDecimal: true,
    decimalSymbol: ',',
    decimalLimit: 2,
    integerLimit: 15,
    allowNegative: false,
    allowLeadingZeroes: false,
};

const defaultMaskOptionsPercent = {
    prefix: '',
    suffix: '%',
    allowDecimal: true,
    decimalLimit: 10,
    integerLimit: 3,
    decimalSymbol: '.',
    allowLeadingZeroes: true,
};

const defaultMaskOptionsDecimals = {
    prefix: '',
    suffix: '',
    allowDecimal: true,
    decimalLimit: 10,
    integerLimit: 3,
    decimalSymbol: '.',
    allowLeadingZeroes: true,
};
const defaultMaskOptionsDecimalsDos = {
    prefix: '',
    suffix: '',
    allowDecimal: true,
    decimalLimit: 18,
    integerLimit: 3,
    decimalSymbol: '.',
    allowLeadingZeroes: true,
};

const defaultMaskOptionsDecimalNumber = {
    prefix: '',
    suffix: '',
    allowDecimal: true,
    decimalLimit: 6,
    integerLimit: 8,
    decimalSymbol: '.',
    allowLeadingZeroes: true,
};

const defaultMaskOptionsDecimalsByNumber = {
    prefix: '',
    suffix: '',
    allowDecimal: true,
    decimalLimit: 2,
    integerLimit: 3,
    decimalSymbol: '.',
    allowLeadingZeroes: true,
};

function NumberInput({ maskOptions, ...inputProps }: any) {
    const currencyMask = createNumberMask({
        ...defaultNumberMaskOptions,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={currencyMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}
function NumberInputTwo({ maskOptions, ...inputProps }: any) {
    const currencyMask = createNumberMask({
        ...defaultNumberOnlyMaskOptions,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={currencyMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function NumberInputInt({ maskOptions, ...inputProps }: any) {
    const currencyMask = createNumberMask({
        ...defaultNumberMaskIntOptions,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={currencyMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function CurrencyInput({ maskOptions, ...inputProps }: any) {
    const currencyMask = createNumberMask({
        ...defaultMaskOptions,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={currencyMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function CurrencyInputDecimal({ maskOptions, ...inputProps }: any) {
    const currencyMask = createNumberMask({
        ...defaultMaskOptionsDecimal,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={currencyMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function CellPhoneInput({ ...inputProps }: any) {
    return (
        <MaskedInput

            mask={[
                '(',
                /[1-9]/,
                /\d/,
                /\d/,
                ')',
                ' ',
                /\d/,
                /\d/,
                /\d/,
                '-',
                /\d/,
                /\d/,
                /\d/,
                /\d/,
            ]}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function PhoneInput({ ...inputProps }: any) {
    return (
        <MaskedInput

            mask={[
                '(',
                /[1-9]/,
                ')',
                ' ',
                /\d/,
                /\d/,
                /\d/,
                '-',
                /\d/,
                /\d/,
                /\d/,
                /\d/,
            ]}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function PercentInput({ maskOptions, ...inputProps }: any) {
    const numberMask = createNumberMask({
        ...defaultMaskOptionsPercent,
        ...maskOptions,
    });
    return (
        <MaskedInput
            mask={numberMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}
function DecimalInput({ maskOptions, ...inputProps }: any) {
    const numberMask = createNumberMask({
        ...defaultMaskOptionsDecimals,
        ...maskOptions,
    });
    return (
        <MaskedInput
            mask={numberMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}
function DecimalInputDos({ maskOptions, ...inputProps }: any) {
    const numberMask = createNumberMask({
        ...defaultMaskOptionsDecimalsDos,
        ...maskOptions,
    });
    return (
        <MaskedInput
            mask={numberMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function DecimalInputNumber({ maskOptions, ...inputProps }: any) {
    const numberMask = createNumberMask({
        ...defaultMaskOptionsDecimalsByNumber,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={numberMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function Anios({ maskOptions, ...inputProps }: any) {
    const numberAniosMask = createNumberMask({
        ...defaultNumberAniosMaskOptions,
        ...maskOptions,
    });
    return (
        <MaskedInput
            mask={numberAniosMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}
function TresDigitos({ maskOptions, ...inputProps }: any) {
    const numberMaskTres = createNumberMask({
        ...defaultnumbermakTresOptions,
        ...maskOptions,
    });
    return (
        <MaskedInput
            mask={numberMaskTres}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}
function Enteros({ maskOptions, ...inputProps }: any) {
    const pesosEnterosMask = createNumberMask({
        ...defaultNumberIntMaskOptions,
        ...maskOptions,
    });
    return (
        <MaskedInput
            mask={pesosEnterosMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function DecimalNumber({ maskOptions, ...inputProps }: any) {
    const numberMask = createNumberMask({
        ...defaultMaskOptionsDecimalNumber,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={numberMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function CHTextField(props: CHTextFieldProps): any {

    const classesInput = useStylesReddit();
    const classesLabel = useStylesLabel();
    const classesContainer = useStylesContainer();
    const [textValue, setTextValue] = useState<string>('');

    useEffect(() => {
        setTextValue(propsTextField.value);
    });

    const {
        isReadOnly,
        currencyMask,
        currencyMaskDecimal,
        cellPhoneMask,
        phoneMask,
        percentMask,
        decimalsMask,
        decimalsMaskDos,
        decimalsMaskNumber,
        decimalsMaskByNumber,
        numberMask,
        numberOnlyMask,
        numberMaskInt,
        nameMask,
        numberAniosMask,
        numberMaskTres,
        pesosEnterosMask,
        currencyMaskOptions,
    } = props;
    const propsTextField = HelperService.omit(props, [
        'isReadOnly',
        'currencyMask',
        'currencyMaskDecimal',
        'cellPhoneMask',
        'phoneMask',
        'percentMask',
        'decimalsMaskNumber',
        'decimalsMask',
        'decimalsMaskDos',
        'decimalsMaskByNumber',
        'numberMask',
        'numberOnlyMask',
        'numberMaskInt',
        'nameMask',
        'numberAniosMask',
        'numberMaskTres',
        'pesosEnterosMask',
        'currencyMaskOptions',
    ]);

    if (currencyMaskDecimal != null) {
        return (
            <CurrencyInputDecimal
                maskOptions={currencyMaskOptions}
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
                onBlur={(e: any) => {
                    let { value } = e.target;

                    if (value) {
                        value = value.replace('$', '');

                        if (!value.includes(',')) {
                            value += ',00';
                        } else {
                            const [entero, decimal] = value.split(',');

                            if (decimal.length === 1) {
                                value = `${entero},${decimal}0`;
                            }
                        }

                        e.target.value = value;

                        if (propsTextField.onChange) {
                            propsTextField.onChange(e);
                        }
                    }
                }}
            />
        );
    }

    if (currencyMask != null) {
        return (
            <CurrencyInput
                maskOptions={currencyMaskOptions}
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,

                }}
                {...propsTextField}
            />
        );
    }

    if (numberAniosMask != null) {
        return (
            <Anios
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }
    if (numberMaskTres != null) {
        return (
            <TresDigitos
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }
    if (pesosEnterosMask != null) {
        return (
            <Enteros
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (cellPhoneMask != null) {
        return (
            <CellPhoneInput
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (phoneMask != null) {
        return (
            <PhoneInput
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (percentMask != null) {
        return (
            <PercentInput
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }
    if (decimalsMask != null) {
        return (
            <DecimalInput
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }
    if (decimalsMaskDos != null) {
        return (
            <DecimalInputDos
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (decimalsMaskNumber != null) {
        return (
            <DecimalNumber
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (decimalsMaskByNumber != null) {
        return (
            <DecimalInputNumber
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (numberMaskInt != null) {
        return (
            <NumberInputInt
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (numberMask != null) {
        return (
            <NumberInput
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (numberOnlyMask != null) {
        return (
            <NumberInputTwo
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
            />
        );
    }

    if (nameMask != null) {
        const handleValidation = (e: any, propsTextField: any) => {
            const reg = new RegExp(/^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s']*$/i);
            if (reg.test(e.target.value)) {
                setTextValue(e.target.value);
                propsTextField.onChange(e.target.value);
            }
        };

        return (
            <TextField
                classes={classesContainer}
                margin="normal"
                InputProps={{
                    classes: classesInput,
                    readOnly: isReadOnly,
                }}
                InputLabelProps={{
                    classes: classesLabel,
                }}
                {...propsTextField}
                value={textValue}
                onChange={e => handleValidation(e, propsTextField)}
            />
        );
    }

    return (
        <TextField
            classes={classesContainer}
            margin="normal"
            InputProps={{
                classes: classesInput,
                readOnly: isReadOnly,
            }}
            InputLabelProps={{
                classes: classesLabel,
            }}
            {...propsTextField}
        />
    );
}

export default CHTextField;
