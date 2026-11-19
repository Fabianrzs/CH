import React from 'react';
import { withStyles } from '@material-ui/core';
import { Autocomplete, AutocompleteProps } from '@material-ui/lab';

const CustomAutocomplete = withStyles({
    root: {
        width: '100%',
    },
})(Autocomplete);

type CHAutoCompleteProps = AutocompleteProps<any, any, any, any>;
function CHAutoComplete(props: CHAutoCompleteProps) {
    return <CustomAutocomplete {...props} />;
}

export default CHAutoComplete;
