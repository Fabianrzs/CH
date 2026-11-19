import React from 'react';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { OperacionesMatriculasDTO } from '../../../../types/realStateRegistrations.type';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

export const ModalMatriculaInmobiliaria = (props: any) => {
    const classes = useStyles();

    const {
        open,
        onClose,
        control,
        listaCausales,
        opcion,
        handleInsertarNovedad,
        hadlOnChangeCausal,
        handleSubmit,
    } = props;

    return (
        <Dialog
            fullWidth
            maxWidth="sm"
            open={open}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
        >
            <DialogContent>
                <Grid item>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        {opcion
                            ? 'BLOQUEAR MATRICULA'
                            : 'DESBLOQUEAR MATRICULA'}
                    </Typography>
                </Grid>
                <br />
                <Grid container spacing={3}>
                    <Grid item xs={3}>
                        <br />
                        <Typography>Causal</Typography>
                    </Grid>
                    <Grid item xs={8}>
                        <Controller
                            name="causalId"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label=""
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={e => {
                                        hadlOnChangeCausal(e.target.value);
                                        onChange(e);
                                    }}
                                >
                                    {listaCausales?.map(
                                        (option: OperacionesMatriculasDTO) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <br />
                <Grid item xs={6}>
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        onClick={handleSubmit(handleInsertarNovedad)}
                    >
                        {opcion ? 'BLOQUEAR' : 'DESBLOQUEAR'}
                    </Button>
                    <Button
                        size="small"
                        variant="contained"
                        color="default"
                        onClick={onClose}
                        style={{
                            marginLeft: '10px',
                        }}
                    >
                        Cancelar
                    </Button>
                </Grid>
            </DialogContent>
        </Dialog>
    );
};
