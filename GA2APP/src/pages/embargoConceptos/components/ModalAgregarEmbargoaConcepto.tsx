import React from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
    FormControlLabel,
    Checkbox,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import moment from 'moment';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
    TipoEmbargoDto,
    TipoRetencionDto,
} from '../../../types/embargo.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);
export const ModalAgregarEmbargosaConceptos = (props: any) => {
    const {
        handleClose,
        open,
        handleOnClickAdd,
        tiposEmbargos,
        control,
        porcentaje,
        valor,
        handleOnChangeMostrarPorcentaje,
        mostrarPorcentaje,
        setErrorPorcentaje,
        setMensaje,
        errorPorcentaje,
        mensaje,
        tiposRetencion,
        handleSubmit,
    } = props;
    const classes = useStyles();
    const today = new Date();
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{ width: '600px' }}>
                    <Grid item>
                        <br />
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            AGREGAR EMBARGO A CONCEPTO
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Tipo Embargo
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="tipoEmbargoId"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <CHSelect
                                        label=""
                                        value={value}
                                        onChange={e => {
                                            if (e.target.value === porcentaje) {
                                                handleOnChangeMostrarPorcentaje(
                                                    true,
                                                );
                                            } else if (
                                                e.target.value === valor
                                            ) {
                                                handleOnChangeMostrarPorcentaje(
                                                    false,
                                                );
                                            }
                                            onChange(e);
                                        }}
                                    >
                                        {tiposEmbargos?.map(
                                            (option: TipoEmbargoDto) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Valor embargo
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            {mostrarPorcentaje ? (
                                <Controller
                                    name="valor"
                                    control={control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido.',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        // fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            // label="Porcentaje Embargo"
                                            value={value}
                                            onChange={e => {
                                                const valor = e.target.value
                                                    .toString()
                                                    .replace('%', '');
                                                if (Number(valor) > 100) {
                                                    setErrorPorcentaje(true);
                                                    setMensaje(
                                                        'El valor no puede ser mayor a 100',
                                                    );
                                                } else if (
                                                    Number(valor) <= 100
                                                ) {
                                                    setErrorPorcentaje(false);
                                                    setMensaje('');
                                                }
                                                onChange(valor);
                                            }}
                                            error={errorPorcentaje}
                                            helperText={mensaje}
                                            percentMask
                                        />
                                    )}
                                />
                            ) : (
                                <Controller
                                    name="valor"
                                    control={control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido.',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                        />
                                    )}
                                />
                            )}
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Fecha de Inicio Embargo
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="fechaInicio"
                                control={control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : ""}
                                        label="Fecha Inicial"
                                        onChange={onChange}
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        autoOk
                                        minDate={new Date("1-01-1950")}
                                        maxDate={today}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Fecha fin Embargo
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="fechaFin"
                                control={control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : ""}
                                        label="Fecha Final"
                                        onChange={onChange}
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        autoOk
                                        minDate={new Date("1-01-1950")}
                                        maxDate={today}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Tipo Retención
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="tipoRetencionId"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label=""
                                        value={value}
                                        onChange={onChange}
                                    >
                                        {tiposRetencion?.map(
                                            (option: TipoRetencionDto) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        {/* <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            ></Typography>
                        </Grid> */}
                        <br />
                        <br />
                        <br />
                        <Grid item xs={3} md={3}>
                            <Controller
                                name="indicadorCesantias"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <FormControlLabel
                                        label="Indicador de Cesantías?"
                                        style={{
                                            fontFamily: 'OpenSans-Regular',
                                            color: 'rgba(0, 0, 0, 0.54)',
                                            fontSize: '1rem',
                                            //
                                        }}
                                        control={
                                            <Checkbox
                                                style={{ marginLeft: '230px' }}
                                                onChange={onChange}
                                                checked={value}
                                                color="primary"
                                            />
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={handleSubmit(handleOnClickAdd)}
                            >
                                Agregar
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={handleClose}
                                style={{ marginLeft: '10px' }}
                            >
                                Cancelar
                            </Button>
                        </Grid>
                    </Grid>
                    <br />
                </DialogContent>
            </Dialog>
        </div>
    );
};
