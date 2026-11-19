import { Grid, MenuItem, TextField, Typography } from "@material-ui/core";
import React, { useEffect } from "react";
import { Controller } from "react-hook-form";
import CHDatePicker from "../../../components/ch-datepicker/CHDatepicker";
import CHSelect from "../../../components/ch-select/CHSelect";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import { CITYDTO, DEPARTAMENTODTO, TIPOINMUEBLE } from "../../../types/catalog.type";

function FormularioInformacionDelInmueble ({
    formualioCartera,
    classes,
    setValorInmueblePP,
    catalogState,
    tipoInmuebles,
    handleOpenAddressDetalleProducto,
    handleKeyPressDetalleProducto,
    filtroCiudadesPorDepartamento,
    filtroTipoViviendaPorCiudad,
    setCambioCiudad,
    ciudades3,
    readonlyFinal,
    bandera,
    salarioMinimoAnual,
}: {
    formualioCartera: any;
    classes: any;
    setValorInmueblePP: any;
    catalogState: any;
    tipoInmuebles: any;
    handleOpenAddressDetalleProducto: any;
    handleKeyPressDetalleProducto:any;
    filtroCiudadesPorDepartamento:any;
    filtroTipoViviendaPorCiudad:any;
    setCambioCiudad:any;
    ciudades3:any;
    readonlyFinal:boolean;
    bandera:any;
    salarioMinimoAnual: any;
   
}) {

    const valorAvaluoWatch = formualioCartera.watch('valorInmueble');

    const limpiarMoneda = (valor: any) => {
        if (!valor) return 0;
    
        return Number(
            String(valor)
                .replace(/\$/g, '')
                .replace(/,/g, '')
                .replace(/\./g, '')
                .trim()
        );
    };
    
    useEffect(() => {
        if (readonlyFinal) return;
    
        const valorAvaluo = limpiarMoneda(valorAvaluoWatch);
    
        const salarioMinimo = Number(
            salarioMinimoAnual?.smlV_SALARIO ??
            salarioMinimoAnual?.smlv_SALARIO ??
            salarioMinimoAnual?.data?.smlV_SALARIO ??
            salarioMinimoAnual?.data?.smlv_SALARIO
        );
    
        const limiteSalariosVis = Number(
            salarioMinimoAnual?.limiteSalariosVis ??
            salarioMinimoAnual?.data?.limiteSalariosVis ??
            salarioMinimoAnual?.vlE_FACTOR_SUBS ??
            salarioMinimoAnual?.data?.vlE_FACTOR_SUBS
        );
    
        if (!valorAvaluo || !salarioMinimo || !limiteSalariosVis) {
            formualioCartera.setValue('idTipoVivienda', '');
            return;
        }
    
        const cantidadSalarios = valorAvaluo / salarioMinimo;
    
        const tipoVIS = catalogState?.tiVivienda?.find((item: any) =>
            String(item.tiV_VIVIENDA_DESCRIPCION).toUpperCase().trim() === 'VIS'
        );
    
        const tipoNoVIS = catalogState?.tiVivienda?.find((item: any) =>
            String(item.tiV_VIVIENDA_DESCRIPCION).toUpperCase().trim() === 'NO VIS'
        );
    
        formualioCartera.setValue(
            'idTipoVivienda',
            cantidadSalarios <= limiteSalariosVis
                ? tipoVIS?.tiV_VIVIENDA_ID
                : tipoNoVIS?.tiV_VIVIENDA_ID
        );
    
    }, [valorAvaluoWatch, salarioMinimoAnual, catalogState?.tiVivienda, readonlyFinal]);

    return (<>
        <Grid className={classes.column} item xs={12} md={12}>
            <Typography className={classes.titleform} variant="h5" component="h6">Información Del Inmueble</Typography>
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="valorInmueblePlanPagos"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
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
                            error ? error.message : null
                        }
                        inputProps={{
                            style: {
                                textAlign: 'right'
                            }

                        }}
                        label="Valor del inmueble *"
                        value={value}
                        onChange={e => {
                            setValorInmueblePP(e.target.value)
                            formualioCartera.setValue('idTipoVivienda', '')
                            onChange(e)
                        }}
                        currencyMask
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="valorInmueble"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
    InputLabelProps={{ shrink: true }}
    error={!!error}
    helperText={error ? error.message : null}
    inputProps={{
        style: {
            textAlign: 'right'
        }
    }}
    label="Valor Avalúo Inmueble*"
    value={value}
    onChange={e => {
        formualioCartera.setValue('idTipoVivienda', '');
        onChange(e);
    }}
    isReadOnly={readonlyFinal}
    currencyMask
/>
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="idTipoVivienda"
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
                    label="Tipo Vivienda *"
                    value={value}
                    error={!!error}
                    helperText={error ? error.message : null}
                    disabled
                >
                        
                        {catalogState.tiVivienda?.map(
                            (option: any) => (
                                <MenuItem
                                    key={option.tiV_VIVIENDA_ID}
                                    value={option.tiV_VIVIENDA_ID}>
                                    {option.tiV_VIVIENDA_DESCRIPCION}
                                </MenuItem>
                            ),
                        )}
                    </CHSelect>
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="idEstadoVivienda"
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
                        label="Estado De Vivienda *"
                        error={!!error}
                        helperText={
                            error ? error.message : null
                        }
                        value={value}
                        onChange={onChange}
                        readOnly= {readonlyFinal}
                    >
                       
                        {catalogState.tipoVivienda?.map(
                            (option: any) => (
                                <MenuItem
                                    key={option.id}
                                    value={option.id}>
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
                name="claseInmueble"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHSelect
                        error={!!error}
                        label="Clase Inmueble *"
                        value={value}
                        readOnly= {readonlyFinal}
                        helperText={
                            error ? error.message : null
                        }
                        onChange={e => {

                            onChange(e)
                        }}
                    >
                        {tipoInmuebles?.map(
                            (option: TIPOINMUEBLE) => (
                                <MenuItem
                                    key={option.descripcion}
                                    value={
                                        option.idValor
                                    }
                                >
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
                name="noMatricula"
                control={formualioCartera.control}
                rules={{
                    required:
                        'Este campo es requerido.',
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
                            error ? error.message : null
                        }
                        label="Matrícula *"
                        value={value}
                        onChange={onChange}
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="escritura"
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
                            error ? error.message || "El número de crédito solo acepta alfanumérico, solo letras, numéros y guión" : null
                        }


                        label="Escritura"
                        value={value}
                        isReadOnly= {readonlyFinal}
                        onChange={onChange}
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                control={formualioCartera.control}
                name="fechaEscritura"
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
                            label="Fecha Escritura*"
                            value={value}
                            disabled= {readonlyFinal}
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
                name="notaria"
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
                            error ? error.message || "El número de crédito solo acepta alfanumérico, solo letras, numéros y guión" : null
                        }
                        label="Notaria*"
                        value={value}
                        isReadOnly = {readonlyFinal}
                        onChange={onChange}
                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="direccionInmueble"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHTextField
                        InputLabelProps={{ shrink: true }}
                        label="Dirección *"
                        error={!!error}
                        helperText={
                            error ? error.message : null
                        }
                        isReadOnly = {readonlyFinal}
                        disabled = {readonlyFinal}
                        value={value}
                        inputProps={{
                            style: {
                                textTransform: 'uppercase',
                            },
                            onClick:
                                handleOpenAddressDetalleProducto,
                        }}
                        onKeyDown={handleKeyPressDetalleProducto}

                    />
                )}
            />
        </Grid>

        <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                name="departamento"
                control={formualioCartera.control}
                rules={{
                    required: 'Este campo es requerido.',
                }}
                render={({
                    field: { onChange, value },
                    fieldState: { error },
                }) => (
                    <CHSelect
                        error={!!error}
                        label="Departamento*"
                        value={value}
                        readOnly = {readonlyFinal}
                        helperText={
                            error ? error.message : null
                        }
                        onChange={value => {
                            
                            const id = value.target.value as number;
                            formualioCartera.setValue('idTipoVivienda', '')
                            filtroCiudadesPorDepartamento(id);
                            onChange(value);
                            formualioCartera.register('ciudadInmueble', {
                                required:
                                    'Este campo es requerido.',
                            });
                        }}
                    >
                      
                        {catalogState?.departments.map(
                            (option3: DEPARTAMENTODTO) => (
                                <MenuItem
                                    key={option3.id}
                                    value={option3.id}
                                >
                                    {option3.nombre}
                                </MenuItem>
                            ),
                        )}
                    </CHSelect>
                )}
            />
        </Grid>

     <Grid className={classes.column} item xs={12} md={3}>
    <Controller
        name="ciudadInmueble"
        control={formualioCartera.control}
        rules={{
            required: 'Este campo es requerido.',
        }}
        render={({
            field: { onChange, value },
            fieldState: { error },
        }) => (
            <CHSelect
                error={!!error}
                label="Ciudad *"
                value={value}
                readOnly = {readonlyFinal}
                helperText={
                    error ? error.message : null
                }
                onChange={value => {
                    onChange(String(value.target.value));
                    filtroTipoViviendaPorCiudad(String(value.target.value));
                    setCambioCiudad(value.target.value)

                }}
            >
                {ciudades3.map(
                    (item3: CITYDTO) => (
                        <MenuItem
                            key={item3.id}
                            value={item3.id}
                        >
                            {item3.nombre}
                        </MenuItem>
                    ),
                )}
            </CHSelect>
        )}
    />
</Grid>
        

        <Grid container spacing={4}>
            <Grid item xs={12} md={12}>
                <Controller
                    name="linderos"
                    control={formualioCartera.control}
                    rules={{
                        required:
                            'Este campo es requerido.',
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <TextField
                            fullWidth
                            label="Linderos*"
                            variant="outlined"
                            value={value}
                            disabled = {readonlyFinal}
                            onChange={onChange}
                            error={!!error}
                            helperText={
                                error ? error.message : null
                            }
                        />
                    )}
                />
            </Grid>
        </Grid>
       
        <br />
        <br />
        <br />
        <br />
        <Grid container spacing={4}>

            <Grid item xs={12} md={12}>
                <Controller
                    name="descripcionInmueble"
                    control={formualioCartera.control}
                    rules={{
                        required:
                            'Este campo es requerido.',
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <TextField
                            fullWidth
                            label="Descripción Inmueble*"
                            variant="outlined"
                            value={value}
                            onChange={onChange}
                            disabled = {readonlyFinal}
                            error={!!error}
                            helperText={
                                error ? error.message : null
                            }
                        />
                    )}
                />
            </Grid>

        </Grid>
    </>)
}
export default FormularioInformacionDelInmueble