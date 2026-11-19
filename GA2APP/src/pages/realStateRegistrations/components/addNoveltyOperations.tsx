
import React, { useEffect } from 'react';
import { Button, createStyles, Dialog, DialogContent, DialogTitle, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import { CausalMatriculaInmobiliariaDto, HistorialNovedadesMatriculasInmobiliarias, OperacionesMatriculasDTO } from '../../../types/realStateRegistrations.type';
import { startAddNoveltyBlocks, startGetOperacionesBloqueos } from '../../../actions/realStateRegistrations.action';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import { startGetCausalBlockage, startGetRequestTypeRealStateRegistration } from '../../../actions/catalog.action';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import AuthService from '../../../services/auth.service';
import CHSelect from '../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '12%',
            marginLeft: '69%',
        },
        button2: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '12%',
            marginLeft: '1%',
        },
        icon: {
            padding: '10px',
            textAlign: 'inherit',
            color: '#1C3F8B',
        },
        title: {
            textAlign: 'center',
        }
    }),
);

function AddNoveltyOperations({
    realStateRegistrationsState,
    isOpen,
    ultimoRegistro,
    onClose,
    startAddNoveltyBlocks,
    startGetCausalBlockage,
    startGetRequestTypeRealStateRegistration,
    consultar
}:
    {
        isOpen: boolean;
        realStateRegistrationsState: RealStateRegistrationsState;
        ultimoRegistro: HistorialNovedadesMatriculasInmobiliarias;
        onClose: (value: any) => void;
        startAddNoveltyBlocks: (agregarNovedadOperacion: HistorialNovedadesMatriculasInmobiliarias) => void;
        startGetCausalBlockage: () => void;
        startGetRequestTypeRealStateRegistration: () => void;
        consultar: () => void;
    }): any {

    useEffect(() => {
        startGetRequestTypeRealStateRegistration();
        startGetCausalBlockage();
    }, []);

    const classes = useStyles();
    const BLOQUEO_ID = "05019aa3-6992-4514-8b03-498a89326078";
    const DESBLOQUEO_ID = "38d1812e-eff8-48ea-9cc7-cdd0c159d0dd";
    const handleClose = () => {
        formularioNovedad.reset();
        const value: any = {};
        onClose(value);
    };

    const nombreUserLogin = (): string => {
        const user = AuthService.getUserInfo();
        return `${user.USR_NOMBRE} ${user.USR_APELLIDO}`;
    };

    const addNoveltyOperations = async (agregarNovedadOperacion: HistorialNovedadesMatriculasInmobiliarias) => {
        const user = AuthService.getUserInfo();
        agregarNovedadOperacion.createPor = user.USR_NOMBRE;
        agregarNovedadOperacion.createPorId = user.USR_ID;
        agregarNovedadOperacion.nombreUsuario = nombreUserLogin();
        startAddNoveltyBlocks(agregarNovedadOperacion)
        consultar()
        formularioNovedad.reset()
        onClose(agregarNovedadOperacion);
    }

    const formularioNovedad =
        useForm<HistorialNovedadesMatriculasInmobiliarias>({
            defaultValues: {
                idNovedad: 'D4F06ADD-D24F-400F-BE9B-0E159D86FC27',
                fechaNovedad: new Date(),
                idMatricula: realStateRegistrationsState.informacionBasica.maiId,
                tipoOperacion: '',
                tipoSolicitudId: 'D4F06ADD-D24F-400F-BE9B-0E159D86FC27',
                IdTipoSolicitud: 0,
                causalId: 0,
            }
        });

    return (
        <Dialog
            fullWidth
            maxWidth="md"
            open={isOpen}
            aria-labelledby="form-dialog-title"
        >
            <DialogTitle className={classes.title} id="form-dialog-title">AGREGAR NOVEDAD </DialogTitle>
            <LoadingComponent key="loader" loading={realStateRegistrationsState.loading} />
            <DialogContent>
                <Grid className="item-containerModal" md={12}>
                    <Grid className='subcontainerModal'>
                        <Controller
                            name="idTipoOperacion"
                            rules={{
                                validate: () => formularioNovedad.watch("idTipoOperacion") === "" || formularioNovedad.watch("idTipoOperacion") == null ?
                                    "Este campo es requerido" : true
                            }}
                            control={formularioNovedad.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de Operación*"
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {realStateRegistrationsState.operacionesBloqueos?.map((option: OperacionesMatriculasDTO) => {
                                        if (ultimoRegistro === null || ultimoRegistro === undefined) {
                                            if (option.descripcion === "BLOQUEO" || option.descripcion === "Bloqueo") {
                                                return (
                                                    <MenuItem key={option.id} value={option.id}>
                                                        {option.descripcion}
                                                    </MenuItem>
                                                );
                                            }
                                        }
                                        else {
                                            if (ultimoRegistro.tipoOperacion === 'Desbloqueo' || ultimoRegistro.tipoOperacion === 'DESBLOQUEO') {
                                                if (option.descripcion !== 'DESBLOQUEO') {
                                                    return (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={option.id}
                                                        >
                                                            {option.descripcion}
                                                        </MenuItem>
                                                    )
                                                }
                                            }
                                            if (ultimoRegistro.tipoOperacion === 'Bloqueo' || ultimoRegistro.tipoOperacion === 'BLOQUEO') {
                                                if (option.descripcion !== 'BLOQUEO') {
                                                    return (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={option.id}
                                                        >
                                                            {option.descripcion}
                                                        </MenuItem>
                                                    )
                                                }
                                            }
                                        }
                                        return null;
                                    })}

                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid className="item-containerModal" md={12}>
                    <Grid className='subcontainerModal'>
                        <Controller
                            name="causalId"
                            control={formularioNovedad.control}
                            rules={{
                                validate: () => formularioNovedad.watch("causalId") === 0?
                                "Este campo es requerido" : true
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de Causal*"
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {realStateRegistrationsState.informacionBasica.causales?.map((option: CausalMatriculaInmobiliariaDto) => {
                                        if (formularioNovedad.watch('idTipoOperacion') === BLOQUEO_ID && option.tipoCausal === 2) {
                                            return (
                                                <MenuItem
                                                    key={option.idCausal}
                                                    value={option.idCausal}
                                                >
                                                    {option.nombreCausal}
                                                </MenuItem>
                                            )
                                        }
                                        if (formularioNovedad.watch('idTipoOperacion') === DESBLOQUEO_ID && option.tipoCausal === 1) {
                                            return (
                                                <MenuItem
                                                    key={option.idCausal}
                                                    value={option.idCausal}
                                                >
                                                    {option.nombreCausal}
                                                </MenuItem>
                                            )
                                        }
                                        return null;
                                    })}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid className="item-containerModal" md={12}>
                    <Grid className='subcontainerModal'>
                        <Controller
                            name="observaciones"
                            control={formularioNovedad.control}
                            rules={{
                                   required: "Este campo es requerido.",
                                   maxLength: {
                                    value: 300,
                                    message: "Máximo 300 caracteres permitidos."
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Observaciones *"
                                    maxRows={5}
                                    multiline
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    inputProps={{
                                        maxLength: 300
                                    }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid >
                    <Button
                        className={classes.button}
                        type="button"
                        onClick={handleClose}
                    >
                        REGRESAR
                    </Button>
                    <Button
                        className={classes.button2}
                        onClick={formularioNovedad.handleSubmit(addNoveltyOperations)}
                        type="submit"
                    >
                        AGREGAR
                    </Button>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}
const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    realStateRegistrationsState: state.realStateRegistrations
});

const mapDispatchToProps = {
    startAddNoveltyBlocks,
    startGetOperacionesBloqueos,
    startGetCausalBlockage,
    startGetRequestTypeRealStateRegistration,
};
export default connect(mapStateToProps, mapDispatchToProps)(AddNoveltyOperations);