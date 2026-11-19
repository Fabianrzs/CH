import React, { useEffect, useState } from 'react';
import { Grid, makeStyles, createStyles, Button, Dialog, DialogContent, DialogTitle, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHDatePickerV2 from '../../../components/ch-datepicker/CHDatepickerV2';
import { DependienteCategoria } from '../../../types/client.model.type';
import AddressComponent from '../../../components/modals/address/address.component'
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { Address } from '../../../types/address.type';
import { AppState } from '../../../store/rootReducer';
import CatalogApi from '../../../services/api-catalog.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },

        icon: {
            padding: '10px',
            textAlign: 'inherit',
            color: '#1C3F8B',
        },

        buttons: {
            color: '#fff',
            width: '120px',
            textAlign: 'center',
            borderRadius: '0%',
            background: '#42b7ef',
            marginBottom: '20px',
            marginLeft: '38rem',
        }

        , title: {
            textAlign: 'center',
            background: '#42b7ef',
            color: '#fff',
        }

        , table: {
            color: '#f1c3b90ff',
        }

    }),
);

const apiCatalog = CatalogApi()

function TramiteDesafiliaciones({
    isOpen,
    onClose,
    onSubmit
}: {
    isOpen: boolean;
    onClose: any;
    onSubmit: any;
}) {
    const classes = useStyles();
    const { control, watch, setValue, handleSubmit, reset } = useForm()
    const [tiposDesafiliacion, setTiposDesafiliacion] = useState<any>([])
    const [modalidades, setModalidades] = useState<any>([])
    const [estadoActualizar, setEstadoActualizar] = useState(false);

    // Lista de IDs permitidos
    const TIPOS_DESAFILIACION_PERMITIDOS = [
        "15ed2bb7-b351-433f-95e7-368dcbebee31", // DESAFILIACIÓN SOLO APORTES
        "33f9682a-6aa5-4cb2-ab97-cbd28aad85d7"  // RETIRO DEFINITIVO
    ];

    const MODALIDADES_PERMITIDAS = [
        "5c757b1b-6b62-42e9-8cdf-d9c191272056", // ACTO ADMINISTRATIVO NO ACREDITACIÓN
        "da821683-34d5-4c58-905b-cf10b517c0f0",  // VOLUNTARIA
        "da3747fe-dc43-4bcd-ac67-131e7400b926", // EXPRESS
        "36d63b8b-6a89-4a6e-9010-f8b5785fc932", // MUERTE DEL AFILIADO
        "4b109588-6f00-414e-9eda-0d11f8b29026" // NORMAL
    ];

    useEffect(() => {
        apiCatalog.ObtenerTipoSolicitudDesafiliacion()
            .then((result: any) => {
                const filtrados = result.data.filter((item: any) =>
                    TIPOS_DESAFILIACION_PERMITIDOS.includes(item.idTipoSolicitud)
                );
                setTiposDesafiliacion(filtrados);
            })
            .catch((err: any) => console.error("Error cargando tipos desafiliación:", err));
    }, []);

    useEffect(() => {
        apiCatalog.getAllTypeModality(watch('idSolicitud'))
            .then((response: any) => {
                const filtrados = response.data.filter((item: any) =>
                    MODALIDADES_PERMITIDAS.includes(item.idTipoModalidad)
                );
                setModalidades(filtrados);
            })
            .catch((err: any) => console.error("Error cargando modalidades:", err));
    }, [watch('idSolicitud')]);

    // useEffect(() => {
        
    //     apiCatalog.ObtenerTipoSolicitudDesafiliacion()
    //         .then((result: any) => {
    //             setTiposDesafiliacion(result.data)
    //         })
    // }, [])

    // useEffect(() => {
        
    //     apiCatalog.getAllTypeModality(watch('idSolicitud'))
    //         .then(res => setModalidades(res.data))
    // }, [watch('idSolicitud')])

    const handleClose = () => {
        onClose()
    }

    const submit = (data: any) => {
        data.descripcionSolicitud = tiposDesafiliacion?.find((x: any) => x.idTipoSolicitud === watch('idSolicitud'))?.descripcionTipoSolicitud as string;
        data.descripcionModalidad = modalidades?.find((x: any) => x.idTipoModalidad === watch('idModalidad'))?.descripcionTipoModalidad as string;
        onSubmit(data)

        setValue('idSolicitud', data.idSolicitud)
        setValue('idModalidad', data.idModalidad)
        setValue('valor', data.valor)

        reset()
    }
    return (
        <Dialog className={classes.table}
            fullWidth
            maxWidth="md"
            open={isOpen}
            aria-labelledby="form-dialog-title"

        >
            <DialogTitle className={classes.title} id="form-dialog-title">AGREGAR TRÁMITE DE DESAFILIACIÓN</DialogTitle>
            <DialogContent>
                <Grid item container>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="idSolicitud"
                                rules={{
                                    validate: () => watch('idSolicitud') < 1 || watch('idSolicitud') == null ? 'Este campo es obligatorio' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo de solicitud*"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}

                                    >
                                        {tiposDesafiliacion.map(
                                            (option: any) => (
                                                <MenuItem key={option.idTipoSolicitud} value={option.idTipoSolicitud}>
                                                    {option.descripcionTipoSolicitud}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="idModalidad"
                                rules={{
                                    validate: () => watch('idModalidad') < 1 || watch('idModalidad') == null ? 'Este campo es obligatorio' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo modalidad*"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}

                                    >
                                        {modalidades?.map(
                                            (option: any) => (
                                                <MenuItem key={option.idTipoModalidad} value={option.idTipoModalidad}>
                                                    {option.descripcionTipoModalidad}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="valor"
                                control={control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es obligatorio'
                                    }
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Valor desafiliación"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        inputProps={{ style: { textAlign: 'right' } }}
                                        currencyMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid >
                        <div className="buttons">
                            <Button
                                className={classes.button}
                                type="button"
                                onClick={handleClose}
                            >
                                REGRESAR
                            </Button>
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="default"
                                style={{ width: '200px' }}
                                onClick={handleSubmit(submit)}
                                type="submit"
                            >
                                AGREGAR
                            </Button>
                        </div>
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}

export default connect()(TramiteDesafiliaciones);


