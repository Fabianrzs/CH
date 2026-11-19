import React from 'react';
import {
    Button,
    Card,
    CardContent,
    Container,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';


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
        column: {
            padding: '20px',

            alignItems: 'flex-end',

            display: 'flex',
            minWidth: '110px',
        },
    }),
);

export const ModalEditarParametrizacionLeasing = (props: any) => {
    const {
        open,
        control,
        handleClose,
        handleOnClickEditar,
        handleSubmit

    } = props;

        const classes = useStyles();
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md" style={{width: '500px'}}>
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >

                                    ACTUALIZAR VALORES POR CATEGORIA
                                </Typography>
                            </Grid>
                            
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="crg_Descripcion"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                            isReadOnly
                                            label="Descripcion *"
                                            InputLabelProps={{
                                                shrink: true,
                                            }}
                                            value={value}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            onChange={onChange}
                                            // inputRef={ModalEditarParametrizacionLeasing} // Agrega esta línea
/>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="cra_Salario_Basico"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                currencyMask
                                                label="Salario Basico *"
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="cra_Salario_Total"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                currencyMask
                                                label="Salario Total *"
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="crg_Factor_Subs"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                
                                                label="Factor Subsidio *"
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={4}>
                                    <Controller
                                        name="crg_ProyectaSM"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <CHCheckbox
                                                label="Activar Proyección SMLV"
                                                onChange={onChange}
                                                checked={value}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={4}>
                                    <Controller
                                        name="crg_ProyectaSubsidio"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <CHCheckbox
                                                label="Activar Proyección Subsidio"
                                                onChange={onChange}
                                                checked={value}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>


                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleOnClickEditar}
                                    >
                                        Actualizar
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
                        </CardContent>
                    </Card>
                </Container>
            </Dialog>
        </div>
    );
};

export default ModalEditarParametrizacionLeasing;
