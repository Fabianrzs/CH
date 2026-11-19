import React from 'react';
import {
    Checkbox,
    CheckboxProps,
    FormControl,
    FormHelperText,
} from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import HelperService from '../../utils/helper.service';

type CHCheckBoxProps = CheckboxProps & {
    label: string;
    helperText?: string | null;
    error?: boolean;
};

const useStyles = makeStyles(() =>
    createStyles({
        root: {
            width: '100%',
            fontSize: '.9rem',
        },
        container: {
            display: 'flex',
            flexDirection: 'row',
            alignItems: 'center',
        },
    }),
);

function CHCheckbox(props: CHCheckBoxProps) {
    const classes = useStyles();
    const { label, error, helperText } = props;
    const propsCheckBox = HelperService.omit(props, ['label']);
    return (
        <FormControl classes={classes} margin="normal">
            <div className={classes.container}>
                <Checkbox color="primary" {...propsCheckBox} />
                <div>{label}</div>
            </div>
            <FormHelperText error={error}>{helperText}</FormHelperText>
        </FormControl>
    );
}
export default CHCheckbox;
