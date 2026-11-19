import React from 'react';
import { withStyles } from '@material-ui/core';
import { Autocomplete, AutocompleteProps } from '@material-ui/lab';

const CustomAutocomplete = withStyles({
    root: {
        width: '100%',
        color: '#42b7ef',
        fontSize: '1.2rem',
    },
})(Autocomplete);

type CHAutoCompleteProps = AutocompleteProps<any, any, any, any>;
function CHAutoCompleteV2(props: CHAutoCompleteProps) {
    return (<CustomAutocomplete {...props}/>);
}

export default CHAutoCompleteV2;
