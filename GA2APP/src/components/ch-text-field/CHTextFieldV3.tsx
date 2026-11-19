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
    cellPhoneMask?: boolean;
    phoneMask?: boolean;
    percentMask?: boolean;
    numberMask?: boolean | null;
    numberZeroMask?: boolean | null;
    nameMask?: boolean;
};

const defaultNumberMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2, // how many digits allowed after the decimal
    integerLimit: 21, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: false,
};

const defaultNumberZeroMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2, // how many digits allowed after the decimal
    integerLimit: 21, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: true,
};

const defaultMaskOptions = {
    prefix: '$',
    suffix: '',
    includeThousandsSeparator: true,
    thousandsSeparatorSymbol: ',',
    allowDecimal: true,
    decimalSymbol: '.',
    decimalLimit: 2, // how many digits allowed after the decimal
    integerLimit: 12, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: false,
};

const defaultMaskOptionsPercent = {
    prefix: '',
    suffix: '%',
    allowDecimal: true,
    decimalLimit: 2,
    integerLimit: 3,
    decimalSymbol: '.',
    allowLeadingZeroes: true,
};



function NumberZeroInput({ maskOptions, ...inputProps }: any) {
    const currencyMask = createNumberMask({
        ...defaultNumberZeroMaskOptions,
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

function CellPhoneInput({ ...inputProps }: any) {
    return (
        <MaskedInput
            // Phone number mask
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
            // Phone number mask
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

function CHTextFieldV3(props: CHTextFieldProps): any {
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
        cellPhoneMask,
        phoneMask,
        percentMask,
        numberMask,
        numberZeroMask,
        nameMask,
    } = props;
    const propsTextField = HelperService.omit(props, [
        'isReadOnly',
        'currencyMask',
        'cellPhoneMask',
        'phoneMask',
        'percentMask',
        'numberMask',
        'numberZeroMask',
        'nameMask',
    ]);
    if (currencyMask != null) {
        return (
            <CurrencyInput
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

    if(numberZeroMask !== null) {
        return (
            <NumberZeroInput
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

export default CHTextFieldV3;
