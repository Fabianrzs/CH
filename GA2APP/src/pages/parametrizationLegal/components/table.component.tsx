import React, { useEffect} from 'react';
import swal from 'sweetalert';
import 'date-fns';
import MaterialTable from 'material-table';
import { connect } from 'react-redux';
import '../../../estiloprincipal.scss';
import {
    Button,
    Container,
    Grid,
    FormControl,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import UtilService from '../../../utils/util.service';
import { AppState } from '../../../store/rootReducer';
import {
    startUpdateLegalReliefParametrization,
    startUpdateLegalGmfParametrization,
    startUpdateLegalRateParametrization,
    startGetLegalRateParametrization,
    startInsertLegalRateParametrization,
    startDeleteLegalRateParametrization,
} from '../../../actions/parametrizationLegal.action';
import { LoginState } from '../../../reducers/login.reducer';
import { ParametrizacionLegalState } from '../../../reducers/parametrizationLegal.reducer';
import {
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
} from '../../../types/parametrizationLegal.model.type';
import { ModalEditar } from './ModalEditarTasa';
import { ModalAgregar } from './ModalAgregarTasa';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';

const TableComponent = (props: Props) => {
    const { parametrizationLegalState,
        load,
        startUpdateLegalReliefParametrization,
        startUpdateLegalGmfParametrization,
        startUpdateLegalRateParametrization,
        startInsertLegalRateParametrization,
        startDeleteLegalRateParametrization,
        startGetLegalRateParametrization,
        loginState} = props;

    const formularioParametrizacionLegal = useForm ({
        defaultValues: {
            id: 0,
            tasaFrech: '',
            vigenciaTasaFrech: new Date(),
            fechaModificacion: new Date(),
            modificadoPor: 0,
            estado: false,
        }
    })

    const formularioAlivioCuota = useForm ({
        defaultValues: {
            id: 0,
            alivioCuota: 0,
            vigenciaAlivioCuota: new Date(),
            fechaModificacion: new Date(),
            modificadoPor: 0,
            estado: false,
        }
    })

    const formularioGmf = useForm ({
        defaultValues: {
            id: 0,
            gmf: 0,
            vigenciaGMF: new Date(),
            fechaModificacion: new Date(),
            modificadoPor: 0,
            estado: false,
        }
    })

    const setData = () => {

        if(parametrizationLegalState.legalRelief){
            formularioAlivioCuota.setValue('id',parametrizationLegalState.legalRelief.id);
            formularioAlivioCuota.setValue('alivioCuota',parametrizationLegalState.legalRelief.alivioCuota);
            formularioAlivioCuota.setValue('vigenciaAlivioCuota',parametrizationLegalState.legalRelief.vigenciaAlivioCuota);
            formularioAlivioCuota.setValue('fechaModificacion',parametrizationLegalState.legalRelief.fechaModificacion);
            formularioAlivioCuota.setValue('modificadoPor',parametrizationLegalState.legalRelief.modificadoPor);
            formularioAlivioCuota.setValue('estado',parametrizationLegalState.legalRelief.estado);
        }
        if(parametrizationLegalState.legalGmf){
            formularioGmf.setValue('id',parametrizationLegalState.legalGmf.id);
            formularioGmf.setValue('gmf',parametrizationLegalState.legalGmf.gmf);
            formularioGmf.setValue('vigenciaGMF',parametrizationLegalState.legalGmf.vigenciaGMF);
            formularioGmf.setValue('fechaModificacion',parametrizationLegalState.legalGmf.fechaModificacion);
            formularioGmf.setValue('modificadoPor',parametrizationLegalState.legalGmf.modificadoPor);
            formularioGmf.setValue('estado',parametrizationLegalState.legalGmf.estado);
        }
      
    };

    useEffect(() => {
        setData();
    }, []);

    const setNewData = () => {
        setTimeout(() => {
            load();
        }, 500);
    };

    const AlivioCuotaPOST = () => (
        <Controller
            name="alivioCuota"
            control={formularioAlivioCuota.control}
            rules={{
                required: 'Este campo es requerido.',
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
                        }) => (
                        <CHTextField
                            decimalsMaskByNumber
                            error={!!error}
                            helperText={
                                error ? error.message : null
                            }
                            InputLabelProps={{ shrink: true }}
                            inputProps={{
                                style: {
                                    textAlign: 'left',
                                },
                            }}
                            label="Alivio Cuota *"
                            value={value}
                            onChange={onChange}
                        />
                )}
        />
    );

    const VigenciaAlivioCuotaPOST = () => (
        <Controller
                name="vigenciaAlivioCuota"
                control={formularioAlivioCuota.control}
                rules={{
                    required: 'Este campo es requerido.'
                }}
                render={({ field: { onChange, value },  
                    fieldState: { error }, 
                }) => (
                    <CHDatePicker
                        label="Vigencia Alivio Cuota *"
                        onChange={onChange}
                        value={value}
                        inputProps={{ readOnly: true }}
                        error={!!error}
                        helperText={
                            error ? error.message : null
                        }
                        autoOk
                    />
                )}
         />
    );

    const GMFPOST = () => (
        <Controller
            name="gmf"
            control={formularioGmf.control}
            rules={{
                required: 'Este campo es requerido.',
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
                        }) => (
                            <CHTextField
                                decimalsMaskByNumber
                                error={!!error}
                                helperText={
                                    error ? error.message : null
                                }
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textAlign: 'left',
                                    },
                                }}
                                label="GMF *"
                                value={value}
                                onChange={onChange}
                            />
                )}
        />
    );

    const VigenciaGMFPOST = () => (
        <Controller
            name="vigenciaGMF"
            control={formularioGmf.control}
            rules={{
                required: 'Este campo es requerido.'
            }}
            render={({ field: { onChange, value },  
                fieldState: { error }, 
            }) => (
                <CHDatePicker
                    label="Vigencia GMF *"
                    onChange={onChange}
                    value={value}
                    inputProps={{ readOnly: true }}
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                    autoOk
                />
            )}
       />
    );

    const handleOnClickDelete = (formStateRateLocal: any) => {
        swal({
            text: '¿Está seguro que desea eliminar el registro?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteLegalRateParametrization(formStateRateLocal);
            }
        });
    };

    const handleOnClickSaveAlivio = (data: any) => {
        swal({
            text: '¿Está seguro que desea Actualizar esta parametrización?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                updateLegalAlivioParametrization(data);
            }
        });
    };

    const handleOnClickSaveGmf = (data: any) => {
        swal({
            text: '¿Está seguro que desea Actualizar esta parametrización?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                updateLegalGmfParametrization(data);
            }
        });
    };


    const deleteLegalRateParametrization = (formStateRateLocal: any) => {
        handleClose1();
        handleClose2();
        startDeleteLegalRateParametrization(
            formStateRateLocal,
            loginState.claims,
        );
        setNewData();
    };

    const insertLegalTasaParametrization = (data: any) => {
        handleClose1();
        handleClose2();
        const datosParametrosLegal = {
            id: 0,
            tasaFrech: Number(data.tasaFrech),
            vigenciaTasaFrech: data.vigenciaTasaFrech,
            fechaModificacion: new Date(),
            modificadoPor: 0,
            estado: false,
        }
        startInsertLegalRateParametrization(
            datosParametrosLegal,
            loginState.claims,
        );
        toast.success('Información Guardada');
        setNewData();
    };

    const updateLegalTasaParametrization = (data: any) => {
        const datosParametrosLegal = {
            id: data.id,
            tasaFrech: Number(data.tasaFrech),
            vigenciaTasaFrech: data.vigenciaTasaFrech,
            fechaModificacion: data.fechaModificacion,
            modificadoPor: data.modificadoPor,
            estado: data.estado,
        }
        handleClose1();
        handleClose2();
        startUpdateLegalRateParametrization(
            datosParametrosLegal,
            loginState.claims,
        );
        toast.success('Información actualizada');
        setNewData();
    };

    const updateLegalAlivioParametrization = (data: any) => {
        const datosAlivioCuota = {
            id: data.id,
            alivioCuota: data.alivioCuota,
            vigenciaAlivioCuota: data.vigenciaAlivioCuota,
            fechaModificacion: new Date(),
            modificadoPor: data.modificadoPor,
            estado: data.estado,
        }

        startUpdateLegalReliefParametrization(
            datosAlivioCuota,
            loginState.claims,
        );
    };

    const updateLegalGmfParametrization = (data: any) => {
        const datosGmf = {
            id: data.id,
            gmf: data.gmf,
            vigenciaGMF: data.vigenciaGMF,
            fechaModificacion: new Date(),
            modificadoPor: data.modificadoPor,
            estado: data.estado,
        }

        startUpdateLegalGmfParametrization(
            datosGmf,
            loginState.claims,
        );
    };

    const columns: any = [
        {
            title: 'Tasa Frech',
            field: 'tasaFrech',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopLeftRadius: '12px',
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Vigencia Tasa Frech',
            field: 'vigenciaTasaFrech',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) => UtilService.getDate(rowData.vigenciaTasaFrech)
        },
    ];

    const [open1, setOpen1] = React.useState(false);
    const [open2, setOpen2] = React.useState(false);

    const handleClickOpen1 = (rowData: any) => {
        formularioParametrizacionLegal.reset();
        setOpen1(true);
        formularioParametrizacionLegal.setValue('id', rowData.id);
        formularioParametrizacionLegal.setValue('tasaFrech', rowData.tasaFrech);
        formularioParametrizacionLegal.setValue('vigenciaTasaFrech', rowData.vigenciaTasaFrech);
        formularioParametrizacionLegal.setValue('vigenciaTasaFrech', rowData.vigenciaTasaFrech);
        formularioParametrizacionLegal.setValue('fechaModificacion', rowData.fechaModificacion);
        formularioParametrizacionLegal.setValue('modificadoPor', rowData.modificadoPor);
        formularioParametrizacionLegal.setValue('estado', rowData.estado);
    };

    const handleClose1 = () => {
        formularioParametrizacionLegal.reset();
        setOpen1(false);
        startGetLegalRateParametrization(loginState.claims);
    };

    const handleClickOpen2 = () => {
        setOpen2(true);
    };

    const handleClose2 = () => {
        setOpen2(false);
    };

    const intercalar = (index: number) => {
        let color = '#f1f2f6';
        if (index % 2 === 0) {
            color = '#C4C4C4';
        }

        return {
            background: color,
            border: '1.5px solid #FFF',
        };
    };

    return (
        <>
                <CHPaper title="Parametrización Legal"> 
                    <Grid container spacing={3}>
                        <Grid item xs={3}>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={handleClickOpen2}
                            >
                                Agregar
                            </Button>
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item xs={12}>
                            <MaterialTable
                                title=""
                                columns={columns}
                                data={parametrizationLegalState.legalRates}
                                actions={[
                                    {
                                        icon: 'edit',
                                        tooltip: 'Editar',
                                        onClick: (event, rowData: any) => {
                                            handleClickOpen1(rowData);
                                        },
                                    },
                                    {
                                        icon: 'delete',
                                        tooltip: 'Eliminar',
                                        onClick: (event, rowData: any) => {
                                            handleOnClickDelete(rowData);
                                        },
                                    },
                                ]}
                                options={{
                                    actionsColumnIndex: -1,
                                    headerStyle: {
                                        backgroundColor: '#FEC917',
                                        fontWeight: 'bold',
                                        color: '#676767',
                                        borderColor: '#FFF',
                                        border: '1.5px solid #FFF',
                                        borderTopRightRadius: '12px',
                                    },
                                    rowStyle: (data, index) => intercalar(index),
                                }}
                                localization={{
                                    header: {
                                        actions: 'Acciones',
                                    },
                                    toolbar: {
                                        searchPlaceholder: 'Buscar',
                                    },
                                }}
                            />
                        </Grid>
                    </Grid>
                <br/>  
                <Container>
                    <Grid container spacing={3}>
                        <Grid item xs={4}>
                            <FormControl fullWidth>
                                <AlivioCuotaPOST />
                            </FormControl>
                        </Grid>
                        <Grid item xs={4}>
                            <FormControl fullWidth>
                                <VigenciaAlivioCuotaPOST />
                            </FormControl>
                        </Grid>
                        <Grid item xs={2}>
                            <FormControl fullWidth>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    style={{
                                        marginTop: '25px'
                                    }}
                                    onClick={formularioAlivioCuota.handleSubmit(handleOnClickSaveAlivio)}
                                >
                                    Actualizar
                                </Button>
                            </FormControl>
                        </Grid>
                    </Grid>
                </Container>
                <Container>
                    <Grid container spacing={3}>
                        <Grid item xs={4}>
                            <FormControl fullWidth>
                                <GMFPOST />
                            </FormControl>
                        </Grid>
                        <Grid item xs={4}>
                            <FormControl fullWidth>
                                <VigenciaGMFPOST />
                            </FormControl>
                        </Grid>
                        <Grid item xs={2}>
                            <FormControl fullWidth>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    style={{
                                        marginTop: '25px'
                                    }}
                                    onClick={formularioGmf.handleSubmit(handleOnClickSaveGmf)}
                                >
                                    Actualizar
                                </Button>
                            </FormControl>
                        </Grid>
                    </Grid>
                </Container>
                <br/>  
                </CHPaper>   
            <ModalEditar
                handleClose={handleClose1}
                open={open1}
                handleOnClickUpdateTasa={updateLegalTasaParametrization}
                formularioParametrizacionLegal={formularioParametrizacionLegal}
            />
            <ModalAgregar
                handleClose={handleClose2}
                open={open2}
                handleOnClickSave={insertLegalTasaParametrization}
                formularioParametrizacionLegal={formularioParametrizacionLegal}
            />
        </>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    parametrizationLegalState: ParametrizacionLegalState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    parametrizationLegalState: state.parametrizationLegal,
});

interface LinkMapDispatchToProps {
    startUpdateLegalReliefParametrization: (
        LegalRelief: LegalAlivioDto,
        props: string,
    ) => void;
    startUpdateLegalGmfParametrization: (
        LegalGmf: LegalGmfDto,
        props: string,
    ) => void;
    startUpdateLegalRateParametrization: (
        LegalRate: LegalTasaDto,
        props: string,
    ) => void;
    startInsertLegalRateParametrization: (
        LegalRate: LegalTasaDto,
        props: string,
    ) => void;
    startDeleteLegalRateParametrization: (
        LegalRate: LegalTasaDto,
        props: string,
    ) => void;
    startGetLegalRateParametrization: (props: string) => void;
    load: () => void;
}

const mapDispatchToProps = {
    startUpdateLegalReliefParametrization,
    startUpdateLegalGmfParametrization,
    startUpdateLegalRateParametrization,
    startGetLegalRateParametrization,
    startInsertLegalRateParametrization,
    startDeleteLegalRateParametrization,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableComponent);
