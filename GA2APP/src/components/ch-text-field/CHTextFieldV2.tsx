import React from 'react';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { TextField, TextFieldProps } from '@material-ui/core';
import MaskedInput from 'react-text-mask';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';
import HelperService from '../../utils/helper.service';

const useStylesContainer = makeStyles(() =>
    createStyles({
        root: {
            width: '98%',
            marginLeft: '0.3rem',
            marginRight: '4rem',
            marginTop: '1%',

        },
    }),
);

const useStylesReddit = makeStyles(() =>
    createStyles({
        input: {
            fontSize: '1.2rem',
            width: '100%',
            marginRight: '1rem',
            marginTop: '3%',
            lineHeight: '1.2em',
        },
    }),
);

const useStylesLabel = makeStyles(() =>
    createStyles({
        root: {
            width: '100%',
            maxWidth: '100%',
            textAlign: 'left',
            color: '#42b7ef',
            fontSize: '1.3rem',
            marginLeft: '1%',
            marginRight: '1%',
        },
    }),
);

type CHTextFieldProps = TextFieldProps & {
    isReadOnly?: boolean;
    currencyMask?: boolean;
    cellPhoneMask?: boolean;
    phoneMask?: boolean;
    percentMask?: boolean;
    numberMask?: boolean;
    accountMask?: boolean;
    identificationMask?: boolean;
    matriculaMask?: boolean;
};

const defaultNumberMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2,
    integerLimit: 11, 
    allowNegative: false,
    allowLeadingZeroes: false,
    allowExponential: false,
};

const defaultAccountOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 0, // how many digits allowed after the decimal
    integerLimit: 21, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: false,
    allowExponential: false,
};

const defaultIdentificationMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2, // how many digits allowed after the decimal
    integerLimit: 12, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: false,
};

const defaultIMatriculaMaskOptions = {
    prefix: '',
    suffix: '',
    includeThousandsSeparator: false,
    thousandsSeparatorSymbol: '.',
    allowDecimal: false,
    decimalSymbol: ',',
    decimalLimit: 2, // how many digits allowed after the decimal
    integerLimit: 12, // limit length of integer numbers
    allowNegative: false,
    allowLeadingZeroes: false,
};

const defaultMaskOptions = {
    prefix: '$',
    suffix: '',
    includeThousandsSeparator: true,
    thousandsSeparatorSymbol: ',',
    allowDecimal: true,
    decimalSymbol: '.',
    decimalLimit: 2, // how many digits allowed after the decimal
    integerLimit: 11, // limit length of integer numbers
    allowNegative: true,
    allowLeadingZeroes: false,
};

const defaultMaskOptionsPercent = {
    prefix: '',
    suffix: '%',
    allowDecimal:true,
    decimalLimit:2,
    integerLimit:3,
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

function AccountInput({ maskOptions, ...inputProps }: any) {
    const accountMask = createNumberMask({
        ...defaultAccountOptions,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={accountMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function IdentificationInput({ maskOptions, ...inputProps }: any) {
    const Identification = createNumberMask({
        ...defaultIdentificationMaskOptions,
        ...maskOptions,
    });

    return (
        <MaskedInput
            mask={Identification}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function MatriculaInput({ maskOptions, ...inputProps }: any) {
    const mask = Array(25).fill(/[A-Za-z0-9]/);

    return (
        <MaskedInput
        mask={mask}
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

function PercentInput({ maskOptions, ...inputProps  }: any) {
    const numberMask = createNumberMask({
       ...defaultMaskOptionsPercent,
       ...maskOptions,
      })
    return (
            <MaskedInput
            mask = {numberMask}
            {...inputProps}
            render={(ref: any, props: any) => (
                <TextField inputRef={ref} {...props} />
            )}
        />
    );
}

function CHTextFieldV2(props: CHTextFieldProps): any {
    const classesInput = useStylesReddit();
    const classesLabel = useStylesLabel();
    const classesContainer = useStylesContainer();

    const { isReadOnly, currencyMask, cellPhoneMask, phoneMask, percentMask, numberMask, accountMask, identificationMask, matriculaMask } = props;
    const propsTextField = HelperService.omit(props, [
        'isReadOnly',
        'currencyMask',
        'cellPhoneMask',
        'phoneMask',
        'percentMask',
        'numberMask',
        'accountMask',
        'identificationMask',
        'matriculaMask'
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

    if (accountMask != null) {
        return (
            <AccountInput
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

    if (identificationMask != null) {
        return (
            <IdentificationInput
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

    if (matriculaMask != null) {
        return (
            <MatriculaInput
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

export default CHTextFieldV2;
