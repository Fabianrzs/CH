import React from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import CHSelect from '../ch-select/CHSelect';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
    }),
);

const RequestInformationDistributionData = (props: TypeProps) => {
    const classes = useStyles();
    const { procedureRequest, isLoading } = props;
    const formularioReparto = useForm({
        defaultValues: {
            finalizaReparto: 'NO',
            fechaFinalizacion: '',
        },
    });

    const ocultar = formularioReparto.watch('finalizaReparto') === 'NO';

    if(isLoading){
        return <CHLoadingProcess/>
    }

    return (
        <>
            <CHPaper title="Información Reparto">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Notaria asignada"
                            value={procedureRequest.activity}
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Fecha de reparto"
                            value={procedureRequest.activity}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="finalizaReparto"
                            control={formularioReparto.control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Finaliza reparto? *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    <MenuItem key="NO" value="NO">
                                        NO
                                    </MenuItem>
                                    <MenuItem key="SI" value="SI">
                                        SI
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaFinalizacion"
                            control={formularioReparto.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value } }) => (
                                <>
                                    <CHDatePicker
                                        hidden={ocultar}
                                        label="Fecha de finalización *"
                                        value={value}
                                        invalidDateMessage="Fecha no válida"
                                        maxDateMessage="Fecha no válida"
                                        placeholder="DD/MM/AAAA"
                                        disableFuture
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
};

type TypeProps = {
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean
};

export default RequestInformationDistributionData;
