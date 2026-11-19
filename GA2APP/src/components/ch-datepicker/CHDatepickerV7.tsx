import React from 'react';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import {
	KeyboardDatePicker,
	MuiPickersUtilsProvider,
} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import esLocale from 'date-fns/locale/es';
import { KeyboardDatePickerProps } from '@material-ui/pickers/DatePicker/DatePicker';

const useStylesReddit = makeStyles(() =>
	createStyles({
		input: {
			fontSize: '.9rem',
			width: '100%',
		},
	}),
);

type CHDatepickerProps = KeyboardDatePickerProps;

function CHDatePickerV7(props: CHDatepickerProps): any {
	const classesInput = useStylesReddit();
	return (
		<MuiPickersUtilsProvider utils={DateFnsUtils} locale={esLocale}>
			<KeyboardDatePicker
				    autoOk
					format="yyyy"
					variant="inline"
					fullWidth
					lang='Es-es'
					placeholder='AAAA'
					KeyboardButtonProps={{
						'aria-label': 'change date',
					}}
					InputProps={{
						classes: classesInput,
					}}
					margin="normal"
					views={['year']}
					{...props}
			/>
		</MuiPickersUtilsProvider>
	);
}

export default CHDatePickerV7;
