import React from 'react';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import {
    KeyboardTimePicker,
    KeyboardTimePickerProps,
    MuiPickersUtilsProvider,
} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import esLocale from 'date-fns/locale/es';

const useStylesReddit = makeStyles(() =>
    createStyles({
        input: {
            fontSize: '.9rem',
            width: '100%',
        },
    }),
);

type CHDatepickerProps = KeyboardTimePickerProps;

function CHTimepicker(props: CHDatepickerProps): any {
    const classesInput = useStylesReddit();
    return (
        <MuiPickersUtilsProvider utils={DateFnsUtils} locale={esLocale}>
            <KeyboardTimePicker
                fullWidth
                KeyboardButtonProps={{
                    'aria-label': 'change date',
                }}
                InputProps={{
                    classes: classesInput,
                }}
                margin="normal"
                {...props}
            />
        </MuiPickersUtilsProvider>
    );
}

export default CHTimepicker;
