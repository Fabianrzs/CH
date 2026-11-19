import React, { useEffect, useState } from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import { FormControl, Icon, IconButton, Input, InputAdornment, InputLabel, TextField, TextFieldProps } from '@material-ui/core';
import HelperService from '../../utils/helper.service';



const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    
    textField: {
      width: '100%',
      marginTop: theme.spacing(2),
    },
  }),
);

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
    icon?: JSX.Element | string ;
    onClickIcon?: () => void;
    numberMask?: boolean | null;
};

function CHTextFieldIcon(props: CHTextFieldProps): any {
    const classesInput = useStylesReddit();
    const classesLabel = useStylesLabel();
    const classesContainer = useStylesContainer();
    const classes = useStyles();
    const [textValue, setTextValue] = useState<string>('');

    useEffect(() => {
        setTextValue(propsTextField.value);
    });

    const {
        isReadOnly,
        onClickIcon,
        icon
    } = props;
    const propsTextField = HelperService.omit(props, [
        'isReadOnly',
        'onClickIcon',
        'icon'
    ]);

    return (
        <FormControl className={classes.textField}>
            <InputLabel >{props.label}</InputLabel>
            <Input
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
                endAdornment={
                <InputAdornment position="end">
                    <IconButton
                    aria-label="toggle button"
                    onClick={onClickIcon}
                    >
                        <Icon>{icon}</Icon>
                    </IconButton>
                </InputAdornment>
                }
            />
        </FormControl>
    );

    
}

export default CHTextFieldIcon;
