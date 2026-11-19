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
                padding: '0.5rem',
                background: '#f9fafc',
                fontSize:'1.3rem',
                color: '#42b7ef',
                marginLeft: '.2rem',
                appearance: 'none',
                cursor: 'pointer',
                ...(!hasLabel && {
                    marginTop: '10px',
                    marginBottom: '4px',
                }),
            },
        }),
    );

const useStylesReddit = makeStyles(() =>
    createStyles({
        root: {
            fontSize: '1.3rem',
            width: '100%',
            color: '#black',
            background: '#f9fafc',
            padding: '0.2rem',
            textAlign: 'left',
        },
    }),
);

type CHSelectProps = SelectProps & {
    label: string;  
    helperText?: string | null;
};
function CHSelectV2(props: CHSelectProps): ReactElement {
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

export default CHSelectV2;
