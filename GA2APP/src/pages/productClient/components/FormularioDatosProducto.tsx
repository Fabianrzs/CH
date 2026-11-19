import { Grid, MenuItem, TextField, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Controller } from "react-hook-form";
import CHDatePicker from "../../../components/ch-datepicker/CHDatepicker";
import CHSelect from "../../../components/ch-select/CHSelect";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import CreditApi from "../../../services/api-credit.service";

const TIPO_CREDITO_LEASING_HABITACIONAL = 1;

function FormularioDatosProducto({
    formualioCartera,
    classes,
    catalogState,
    setEsLeasing,
    starGetCreditLine,
    readonlyFinal,
    productoDatos,
    categoriaSalario,
    setSalarioMinimoAnual,
}: {
    formualioCartera: any;
    classes: any;
    catalogState: any;
    setEsLeasing: any;
    starGetCreditLine: any;
    readonlyFinal: boolean;
    productoDatos: any;
    categoriaSalario?: number | null;
    setSalarioMinimoAnual?: React.Dispatch<React.SetStateAction<any>>;

}) {

    const creditApi = CreditApi();

    const fechaDesembolsoInicialWatch = formualioCartera.watch('fechaDesembolso');

 

    useEffect(() => {
        const consultarSalario = async () => {
            if (!categoriaSalario || !fechaDesembolsoInicialWatch || !setSalarioMinimoAnual) return;
    
            const anio = new Date(fechaDesembolsoInicialWatch).getFullYear();
    
            try {
                const salarioResponse = await creditApi.getObtenerSalarioMinimoAnual(
                    anio,
                    categoriaSalario
                );
    
                setSalarioMinimoAnual(
                    salarioResponse?.data?.data ?? salarioResponse?.data
                );
            } catch (error) {
                console.log('Error consultando salario mínimo anual:', error);
                setSalarioMinimoAnual(null);
            }
        };
    
        consultarSalario();
    }, [categoriaSalario, fechaDesembolsoInicialWatch, setSalarioMinimoAnual]);
    

        useEffect(() => {
            if( productoDatos!== null){
                starGetCreditLine(Number(productoDatos.tipoCredito))
                formualioCartera.setValue('lineaCredito',productoDatos?.lineaCredito )
                setEsLeasing(Number(productoDatos.tipoCredito) === TIPO_CREDITO_LEASING_HABITACIONAL)
               
            }
        },[productoDatos !== null])

    return (<>
        <Grid className={classes.column} item xs={12} md={12}>
            <Typography className={classes.titleform} variant="h5" component="h6">Datos Producto</Typography>
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="numeroCredito"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                    pattern: /^[a-zA-Z\-0-9]+$/g

                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        InputLabelProps={{ shrink: true }}
                        error={!!error}
                        isReadOnly = {readonlyFinal}
                        helperText={
                            error ? error.message || "El número de crédito solo acepta alfanumérico, solo letras, numéros y guión" : null
                        }
                        label="Numero de Crédito*"
                        value={value}
                        onChange={(onChange)}
                        numberOnlyMask

                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="idTipoCredito"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHSelect

                        label="Tipo De Crédito*"
                        value={value}
                        error={!!error}
                     readOnly= {readonlyFinal}
                        helperText={
                            error ? error.message : null
                        }
                        onChange={(value) => {
                            if(!readonlyFinal){
                                starGetCreditLine(Number(value.target.value))
                                setEsLeasing(Number(value.target.value) === TIPO_CREDITO_LEASING_HABITACIONAL)
                            }
                         
                           
                            onChange(value)
                        }}
                    >
                        {catalogState.tipoCredito?.map(
                            (option: any) => (
                                <MenuItem key={option.tcR_ID} value={option.tcR_ID}>
                                    {option.tcR_DESCRIPCION}
                                </MenuItem>
                            ),
                        )}
                    </CHSelect>
                )}
            />

        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="lineaCredito"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHSelect
                         readOnly= {readonlyFinal}
                        label="Linea de Crédito*"
                        value={value}
                        error={!!error}
                        helperText={
                            error ? error.message : null
                        }
                        onChange={(value) => {
                            onChange(value)
                        }}
                       
                    >
                       
                        {catalogState.Line?.map(
                            (option: any) => (
                                <MenuItem
                                    key={option.idLineaCredito}
                                    value={option.idLineaCredito}>
                                    {option.descripcion}
                                </MenuItem>
                            ),
                        )}
                    </CHSelect>
                )}
            />

        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                control={formualioCartera.control}
                name="fechaDesembolso"
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <>
                        <CHDatePicker
                            error={!!error}
                            helperText={error ? error.message : null}
                            label="Fecha Desembolso Inicial*"
                            value={value}
                            disabled = {readonlyFinal}
                            onChange={date => {
                                onChange(date);
                            }}
                        />

                    </>
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                control={formualioCartera.control}
                name="fechaSolicitud" 
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <>
                        <CHDatePicker
                            error={!!error}
                            helperText={
                                error ? error.message : null
                            }
                            label="Fecha Desembolso Final*"
                            value={value}
                            disabled = {readonlyFinal}
                            onChange={date => {
                                onChange(date);
                            }}
                        />

                    </>
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="valorDesembolso"
                control={formualioCartera.control}
                rules={{
                    validate: (value) => value !== 0 || 'Este campo es requerido si el valor es 0.',

                    required:
                        'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        error={!!error}
                        InputLabelProps={{ shrink: true }}
                        helperText={
                            error ? error.message : null
                        }
                        inputProps={{
                            style: {
                                textAlign: 'right'
                            }

                        }}
                        label="Valor Desembolso*"
                        value={value}
                        isReadOnly = {readonlyFinal}
                        onChange={onChange}
                        currencyMask

                    />
                )}
            />

        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="valorCanonInicial"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo es requerido.',
                    min: {
                        value: 0,
                        message: 'No puede ser menor a 0'
                    },
                    max: {
                        value: 100,
                        message: 'No puede ser mayor a 100'
                    },
                    validate: value =>
                        (value >= 0 && value <= 100) || 'El valor debe estar entre 0 y 100',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        error={!!error}
                        helperText={error ? error.message : null}
                        // inputProps={{
                        //     style: {
                        //         textAlign: 'right',
                        //     },
                        // }}
                        label="Canon Inicial (%)"
                        value={value}
                        onChange={(e) => {
                            const newValue = e.target.value.replace('%', '');
                            onChange(newValue);
                        }}
                        InputLabelProps={{ shrink: true }}
                        percentMask
                        isReadOnly = {readonlyFinal}
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="porcentajeOpcionCompra"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo es requerido.',
                    min: {
                        value: 0,
                        message: 'No puede ser menor a 0'
                    },
                    max: {
                        value: 100,
                        message: 'No puede ser mayor a 100'
                    },
                    validate: value =>
                        (value >= 0 && value <= 100) || 'El valor debe estar entre 0 y 100',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        error={!!error}
                        helperText={error ? error.message : null}
                        // inputProps={{
                        //     style: {
                        //         textAlign: 'right',
                        //     },
                        // }}
                        label="Opción De Compra*"
                        value={value}
                        isReadOnly = {readonlyFinal}
                        onChange={(e) => {
                            const newValue = e.target.value.replace('%', '');
                            onChange(newValue);
                        }}
                        InputLabelProps={{ shrink: true }}
                        percentMask
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="tipoAbonoExtra"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo requerido',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        error={!!error}
                        helperText={error ? error.message : null}
                        label="Tipo Abono Extra"
                        value={value}
                        onChange={onChange}
                        // isReadOnly = {readonlyFinal}
                        InputLabelProps={{ shrink: true }}
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="valorAbonoExtra"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo requerido',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        inputProps={{
                            style: {
                                textAlign: 'right',
                            },
                        }}
                        error={!!error}
                        helperText={error ? error.message : null}
                        label="Valor Abono Extra"
                        isReadOnly = {readonlyFinal}
                        value={value}
                        // value={obtnerProductoTablaFilter[0]?.valorPago.toFixed(0) ?? 0}
                        onChange={onChange}
                        currencyMask
                        InputLabelProps={{ shrink: true }}
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="tasaCreditoEA"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                    min: {
                        value: 0,
                        message: 'No puede ser menor a 0'
                    },
                    max: {
                        value: 100,
                        message: 'No puede ser mayor a 100'
                    },
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        error={!!error}
                        InputLabelProps={{ shrink: true }}
                        helperText={
                            error ? error.message || 'No es un número valido' : null
                        }
                        label="Tasa Crédito (EA)*"
                        value={value}
                        isReadOnly = {readonlyFinal}
                        onChange={(e) => {
                            const newValue = e.target.value.replace('%', '');
                            onChange(newValue);
                        }}
                        percentMask
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="score"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                    pattern: /^[a-zA-Z\-0-9]+$/g

                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        InputLabelProps={{ shrink: true }}
                        error={!!error}
                        helperText={
                            error ? error.message : null
                        }
                        label="Score*"
                        value={value}
                        onChange={(onChange)}
                        isReadOnly = {readonlyFinal}
                        numberMaskInt
                    />
                )}
            />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="Calificacion"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo es requerido.',
                    pattern: {
                        value: /^[a-zA-Z]$/,
                        message: 'Solo se permite una letra.'
                    }
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        InputLabelProps={{ shrink: true }}
                        error={!!error}
                        isReadOnly = {readonlyFinal}
                        helperText={error ? error.message : null}
                        label="Calificación*"
                        value={value}
                        onChange={(e) => {
                            const newValue = e.target.value;
                            // If newValue has more than one character, take only the first one
                            if (newValue.length > 1) {
                                onChange(newValue.charAt(0));
                            } else {
                                onChange(newValue);
                            }
                        }}
                    />
                )}
            />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="plazoTotal"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                    pattern: /^[a-zA-Z\-0-9]+$/g

                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        InputLabelProps={{ shrink: true }}
                        error={!!error}
                        isReadOnly = {readonlyFinal}
                        helperText={
                            error ? error.message || "El plazo solo acepta alfanumérico solo letras, numéros y guión" : null
                        }
                        label="Plazo Total*"
                        value={value}
                        onChange={(onChange)}
                        numberMaskInt

                    />
                )}
            />
        </Grid>
    </>)
}

export default FormularioDatosProducto