import React, { useState } from 'react';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    Typography,
    Theme,
} from '@material-ui/core';
import swal from 'sweetalert';
import MaterialTable from 'material-table';
import CheckIcon from '@material-ui/icons/Check';
import { useForm } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import { ModalAgregarTasaEA } from './ModalAgregarTasaEA';
import { ModalEditarTasaEA } from './ModalEditarTasaEA';
import { AppState } from '../../../store/rootReducer';
import {
    startPostInsertSimuladorTasa,
    startPostUpdateSimuladorTasas,
    startPostDeleteSimuladorTasas,
} from '../../../actions/simulador.action';
import { SimuladorState } from '../../../reducers/simulador.reducer';
import { SimuladorDto } from '../../../types/simulador.model.type';
import TasaUsuraMora from './TasaUsuraMora';
import ApiSimuladorTasas from '../../../services/api-simulador.service';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        checkContainer: {
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            paddingLeft: 30,
            color: theme.palette.success.main,
        },
        inactiveContainer: {
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            paddingLeft: 45,
            color: '#C44',
        },
        buttonAdd: {
            width: '200px',
            marginBottom:  '1.5rem',
            marginTop: '.5rem',
           

        },
    }),
);

const ParametrizationMortgageCredit = (props: any) => {
    const simuladorTasas = ApiSimuladorTasas()

    const classes = useStyles();
    const {
        simuladorState,
        load,
        startPostInsertSimuladorTasa,
        startPostUpdateSimuladorTasas,
        startPostDeleteSimuladorTasas,
    } = props;
   
    const columns: any = [
        {
            title: 'Descripción',
            field: 'descripcion',
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
            title: 'Tasa EA (%)',
            field: 'tasaEA',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },

        {
            title: 'Tasa NM (%)',
            field: 'tasaEA',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
      
        {
            title: 'Plazo Mínimo (Meses)',
            field: 'minimoPlazoMeses',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Plazo Máximo (Meses)',
            field: 'maximoPlazoMeses',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Mínimo cánon inicial',
            field: 'min_Canon_Inicial',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {          
            title: 'Maximo cánon inicial (30%)',
            field: 'max_Canon_Inicial',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Máximo opción de compra (%)',
            field: 'max_Opc_Compra',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Seguro Hogar',
            field: 'seguroHogar',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Estado',
            field: 'estado',
            align: 'center',
            render: (rowData: any) => (
                <>
                    {rowData.estado === '1' ? (
                        <span className={classes.checkContainer}>
                            <b>
                                <CheckIcon /> ACTIVO
                            </b>
                        </span>
                    ) : (
                        <span className={classes.inactiveContainer}>
                            <b>INACTIVO</b>
                        </span>
                    )}
                </>
            ),
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
    ];

    const formularioParametrosSimulador = useForm({
        defaultValues: {
            idSimulacion: '',
            descripcion:'',
            tasaNM: '',
            tasaEA: '',
            minimoPlazoMeses: 0,
            maximoPlazoMeses: 0,
            estado: '1',
            min_Canon_Inicial: 0,
            max_Canon_Inicial: 0,
            max_Opc_Compra: 0,
            seguroHogar: '',
        }
    })

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalAgregarTasaUsura, setOpenModalAgregarTasaUsura] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);

    const handleOpenModalAgregar = () => {
        formularioParametrosSimulador.reset();
        setOpenModalAgregar(true);
    };


    const handleCloseModalAgregar = () =>  setOpenModalAgregar(false);

    const handleOpenModalEditar = (simulador: any) => {

        formularioParametrosSimulador.reset();

        formularioParametrosSimulador.setValue('idSimulacion',simulador.idSimulacion);
        formularioParametrosSimulador.setValue('descripcion',simulador.descripcion);
        formularioParametrosSimulador.setValue('tasaEA',simulador.tasaEA);
        formularioParametrosSimulador.setValue('minimoPlazoMeses',simulador.minimoPlazoMeses);
        formularioParametrosSimulador.setValue('maximoPlazoMeses',simulador.maximoPlazoMeses);
        formularioParametrosSimulador.setValue('estado',simulador.estado);
        formularioParametrosSimulador.setValue('min_Canon_Inicial',simulador.min_Canon_Inicial);
        formularioParametrosSimulador.setValue('max_Canon_Inicial',simulador.max_Canon_Inicial);
        formularioParametrosSimulador.setValue('max_Opc_Compra',simulador.max_Opc_Compra);
        formularioParametrosSimulador.setValue('seguroHogar',simulador.seguroHogar);
 
        setOpenModalEditar(true);

    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };

    const setNewData = async () => {
        await load();
    };

    const handleOnClickAgregar = async (data: any) => {
        if(String(data.descripcion).toLocaleLowerCase().includes('hipotecario') ){
            if(data.minimoPlazoMeses > 60){
                swal({
                    text: 'Plazo Mínimo puede ser hasta 60 meses',
                    icon: 'warning',
                });
                return;
            }
            // if(data.maximoPlazoMeses > 180){
            //     swal({
            //         text: 'Plazo Máximo no puede ser mayor a 180 meses',
            //         icon: 'warning',
            //     });
            //     return;
            // }
        }

        if(String(data.descripcion).toLocaleLowerCase().includes('leasing') ){
            if(data.minimoPlazoMeses > 60){
                swal({
                    text: 'Plazo Mínimo puede ser hasta 60 meses',
                    icon: 'warning',
                });
                return;
            }
            if(data.maximoPlazoMeses > 999){
                swal({
                    text: 'Plazo Máximo no puede ser mayor a 999 meses',
                    icon: 'warning',
                });
                return;
            }
        }

        if(Number(data.minimoPlazoMeses) > Number(data.maximoPlazoMeses)){
            swal({
                text: 'Plazo Mínimo no puede ser mayor que plazo Máximo',
                icon: 'warning',
            });
            return;
        }

        const request: any = {
            idSimulacion: '',
            descripcion: data.descripcion,
            tasaEA: data.tasaEA,
            minimoPlazoMeses: data.minimoPlazoMeses,
            maximoPlazoMeses: data.maximoPlazoMeses,
            estado: '1', 
            min_Canon_Inicial: data.min_Canon_Inicial,
            max_Canon_Inicial: data.max_Canon_Inicial,
            max_Opc_Compra: data.max_Opc_Compra,
            seguroHogar: data.seguroHogar,

        }
        await simuladorTasas
        .postInsertSimuladorTasas(
            request
        )
        .then(async response => {
            if (response) {
                const array = response.data?.cuentas;
                if (response.isSuccess ) {
                    const respuesta = response.data
                }
            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });

    
        handleCloseModalAgregar();
        setNewData();
        toast.success('Información Guardada');
    };

    const handleOnClickEditar = async (data: any) => {

        if(String(data.descripcion).toLocaleLowerCase().includes('hipotecario') ){
            if(data.minimoPlazoMeses > 60){
                swal({
                    text: 'Plazo Mínimo puede ser hasta 60 meses',
                    icon: 'warning',
                });
                return;
            }
            // if(data.maximoPlazoMeses > 180){
            //     swal({
            //         text: 'Plazo Máximo no puede ser mayor a 180 meses',
            //         icon: 'warning',
            //     });
            //     return;
            // }
        }

        if(String(data.descripcion).toLocaleLowerCase().includes('leasing') ){
            if(data.minimoPlazoMeses > 60){
                swal({
                    text: 'Plazo Mínimo puede ser hasta 60 meses',
                    icon: 'warning',
                });
                return;
            }
            if(data.maximoPlazoMeses > 999){
                swal({
                    text: 'Plazo Máximo no puede ser mayor a 999 meses',
                    icon: 'warning',
                });
                return;
            }
        }

        if(Number(data.minimoPlazoMeses) > Number(data.maximoPlazoMeses)){
            swal({
                text: 'Plazo Mínimo no puede ser mayor que plazo Máximo',
                icon: 'warning',
            });
            return;
        }

        const simulador: SimuladorDto = {
            idSimulacion: data.idSimulacion,
            descripcion: data.descripcion,
            tasaEA: data.tasaEA,
            minimoPlazoMeses: data.minimoPlazoMeses,
            maximoPlazoMeses: data.maximoPlazoMeses,
            estado: data.estado, 
            min_Canon_Inicial: data.min_Canon_Inicial,
            max_Canon_Inicial: data.max_Canon_Inicial,
            max_Opc_Compra: data.max_Opc_Compra,
            seguroHogar: data.seguroHogar,
            
        }

        const confirm = await swal({
            title: 'Confirmación',
            text: '¿Está seguro de que desea actualizar la información?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
            dangerMode: true,
        });
        
        if (!confirm) {
            return;
        }

        await simuladorTasas
        .postUpdateSimuladorTasas(
            simulador
        )
        .then(async response => {
            if (response) {
                const array = response.data?.cuentas;
                if (response.isSuccess ) {
                    const resp = response.dato
                   
                }
            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });

        setNewData();
        toast.success('Información Guardada');
        setOpenModalEditar(false);

    }

    const deleteState = async (simulador: SimuladorDto) => {
        await startPostDeleteSimuladorTasas(simulador);
        setNewData();
        toast.success('Estado modificado con éxito');
    };

    const handleOnClickEliminar = (rowData: any) => {
        const simulador = rowData as SimuladorDto;
        swal({
            text: 'El estado del registro se modificará como INACTIVO.¿Desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteState(simulador);
            }
        });
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
        <CHPrivateContainer>
            <CHPaper title="Parámetros de Crédito">
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Parámetros de simulación de crédito
                    </Typography>
                </Grid>
                <hr />
                <Grid container>
                    <Button             
                        className={classes.buttonAdd}
                        size="large"
                        variant="contained"
                        color="primary"
                        onClick={handleOpenModalAgregar}
                    >
                        Agregar
                    </Button>
                </Grid>
                <Grid container>
                    <Grid item xs={12}>
                        <MaterialTable
                            style={{
                                borderTopLeftRadius: '12px',
                                borderTopRightRadius: '12px',
                            }}
                            title=""
                            data={simuladorState.simuladorGeneral}
                            columns={columns}
                            actions={[
                                {
                                    icon: 'edit',
                                    tooltip: 'Editar',
                                    onClick: ( event,rowData: any) => {
                                        handleOpenModalEditar( rowData);
                                    },
                                },
                                {
                                    icon: 'delete',
                                    tooltip: 'Eliminar',
                                    onClick: ( rowData: any) => {
                                        if(rowData.estado === '0'){
                                            swal({
                                                text: 'El estado del registro es INACTIVO',
                                                icon: 'warning',
                                            });
                                        }
                                        else{
                                            handleOnClickEliminar(rowData);
                                        }
                                       
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
                                search: true,
                                toolbar: true,
                                rowStyle: (data:any , index:any) => intercalar(index),
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
                
            </CHPaper>
            {/* <TasaUsuraMora/> */}
            <ModalAgregarTasaEA
                handleClose={handleCloseModalAgregar}
                open={openModalAgregar}
                handleOnClickSave={handleOnClickAgregar}
                formularioParametrosSimulador={formularioParametrosSimulador}
            />
            <ModalEditarTasaEA
                handleClose={handleCloseModalEditar}
                open={openModalEditar}
                handleOnClickUpdateTasa={handleOnClickEditar}
                formularioParametrosSimulador={formularioParametrosSimulador}
            />
            
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    simuladorState: SimuladorState;
}

const mapStateToProps = (state: AppState) => ({
    simuladorState: state.simulador,
});

interface LinkMapDispatchToProps {
    startPostInsertSimuladorTasa: (simulador: SimuladorDto) => void;
    startPostUpdateSimuladorTasas: (simulador: SimuladorDto) => void;
    startPostDeleteSimuladorTasas: (simulador: SimuladorDto) => void;
    load: () => void;
}

const mapDispatchToProps = {
    startPostInsertSimuladorTasa,
    startPostUpdateSimuladorTasas,
    startPostDeleteSimuladorTasas,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ParametrizationMortgageCredit);
