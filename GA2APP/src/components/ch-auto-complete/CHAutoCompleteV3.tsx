import React from 'react';
import { FormControl, FormHelperText, Omit, withStyles } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { Autocomplete, AutocompleteProps } from '@material-ui/lab';

const CustomAutocomplete = withStyles({
    root: {
        width: '100%',
        marginBottom: '-6px',
        boxSizing: 'border-box'
    },
})(Autocomplete);

const formControlClasses = makeStyles(() =>
    createStyles({
        root: {
            width: '100%',
        },
    }),
);


type CHAutoCompleteProps = AutocompleteProps<any, any, any, any> & {
    helperText?: string | null;
    error?: boolean;
    inputProps?: any;
};

function CHAutoCompleteV3(props: CHAutoCompleteProps) {
    const { error, helperText } = props;
    const classes = formControlClasses();
    const autoCompleteProps = (({helperText, error, ...properties}) => properties)(props);

    return <FormControl classes={classes}>
        <CustomAutocomplete size='small' {...autoCompleteProps} />
        <FormHelperText error={error}>{helperText}</FormHelperText>
    </FormControl>;
}

export default CHAutoCompleteV3;
