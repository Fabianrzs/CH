import React, { ReactElement } from 'react';
import Select, { SelectProps } from '@material-ui/core/Select';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { FormControl, FormHelperText, InputLabel } from '@material-ui/core';
import { omit } from 'lodash';

const useStylesContainer = (hasLabel: boolean) =>
    makeStyles(() =>
        createStyles({
            root: {
                width: '100%',
                textAlign: 'left',
                ...(!hasLabel && {
                    marginTop: '20px',
                    marginBottom: '-2px',
                }),
            },
        }),
    );

const useStylesReddit = makeStyles(() =>
    createStyles({
        root: {
            fontSize: '.9rem',
            width: '100%',
        },
    }),
);

type CHSelectProps = SelectProps & {
    label: string;  
    helperText?: string | null;
};
function CHSelect(props: CHSelectProps): ReactElement {
    const classesInput = useStylesReddit();
    const { label, children, helperText, error } = props;
    const classes = useStylesContainer(label !== '')();
    const propsSelect = omit(props, ['helperText']);

    return (
        <FormControl classes={classes} margin="normal">
            {label !== '' && (
                <InputLabel shrink classes={classes} error={error}>
                    {label}
                </InputLabel>
            )}
            <Select classes={classesInput} {...propsSelect}>
                {children}
            </Select>
            <FormHelperText error={error}>{helperText}</FormHelperText>
        </FormControl>
    );
}

export default CHSelect;
