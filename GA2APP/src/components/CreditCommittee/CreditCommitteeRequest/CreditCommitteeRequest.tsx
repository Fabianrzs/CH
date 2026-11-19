import React, { useEffect, useState } from 'react';
import MUIDataTable from 'mui-datatables';
import { useForm } from 'react-hook-form';
import { Button, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import moment from 'moment';
import CHPaper from '../../ch-paper/CHPaper';
import { AppState } from '../../../store/rootReducer';
import {
    startPostCreateCreditCommitteeRequest,
    startPostCreditCommitteeRequestSuccess,
} from '../../../actions/credit.actions';
import { CreditCommitteeModel } from '../../../models/credit-committee.model';
import CHLoadingProcess from '../../ch-loading-process/CHLoadingProcess';
import { ModalCreditCommitee } from '../Modal/ModalCreditCommitee';
import ApiCredit from '../../../services/api-credit.service';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';
import NotificationService from '../../../services/notification.service';

function CreditCommitteeRequest({
    onItemGoBack,
    creditCommittee,
    startPostCreditCommitteeRequestSuccess,
    startPostCreateCreditCommitteeRequest,
    listaSolicitudes,
    list,
    isLoading,
    isSuccess,
    obtenerTramitesPendientes,
    mostrarFormularioComite,
    mostrarListaSolicitudes,
    mostrarBotonProcesar,
    setMostrarFormularioComite,
    setMostrarListaSolicitudes, 
    setMostrarBotonProcesar
}: {
    onItemGoBack: () => void;
    startPostCreditCommitteeRequestSuccess: (request: any) => void;
    startPostCreateCreditCommitteeRequest: (request: any) => void;
    obtenerTramitesPendientes: () => void;
    creditCommittee: CreditCommitteeModel;
    list: any[];
    listaSolicitudes: any[];
    isLoading: boolean;
    isSuccess?: boolean;
    mostrarFormularioComite: boolean;
    mostrarListaSolicitudes: boolean;
    mostrarBotonProcesar: boolean;
    setMostrarFormularioComite: (value: boolean) => void;
    setMostrarListaSolicitudes: (value: boolean) => void;
    setMostrarBotonProcesar:  (value: boolean) => void;
}) {

    const apiCredit = ApiCredit();
    const array  = listaSolicitudes;
    const [index,setIndex] = useState<any>(null);
    const [causalesRechazo,setCausalesRechazo] = useState<any[]>([]);
    const [estadoSolicitud,setEstadoSolicitud] = useState<any[]>([]);
    const [valorSolicitud,setValorSolicitud] = useState<any>(null);
    const [resultado,setResultado] = useState<any[]>(listaSolicitudes);

  

    const [openModal,setOpenModal] = useState(false);

    const formulario = useForm ({
        defaultValues:{
            resultado: '',
            causal:''
        }
    })

    // const handleGoBack = () => {
    //     setMostrarFormularioComite(true);
    //     setMostrarListaSolicitudes(true);
    //     setMostrarBotonProcesar(false);
    // }

    const handleOpenModal = () => {
        setOpenModal(true);
        obtenerEstadosSolicitud();
        obtenerCausalesRechazo();
    }
    const handleCloseModal = () => setOpenModal(false);
    const [inlineValues, setInlineValues] = useState<any[]>([]);

     const handleProcess = (index: any) => () => {
        handleOpenModal();
        const item = array[index];
  
        setValorSolicitud(item);
        setIndex(index);
    };

    const obtenerCausalesRechazo = async () => {
        await apiCredit
            .getObtenerCausalesRechazo()
            .then(async response => {
                if (response) {
                    if (response.isSuccess && response.data) {
                        setCausalesRechazo(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerEstadosSolicitud = async () => {
        const array: any[]=[];
        await apiCredit
            .getObtenerEstadosTramite()
            .then(async response => {
                if (response) {
                    if (response.isSuccess && response.data) {
                        response.data.forEach((item:any)  => {
                            
                            if(item.descripcion === 'APROBADA' || item.descripcion === 'RECHAZADA')
                            array.push(item);
                        })
                        setEstadoSolicitud(array);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleOnClick = (data: any) => {
       
            const user = AuthService.getUserInfo();
           const request = {
            // idSolicitudCredito: "00000000-0000-0000-0000-000000000000",
            idSolicitudCredito: valorSolicitud?.solicitudId,
            idComiteCredito: creditCommittee.codigoComite,
            resultado: data.resultado === 7? "APROBADA" : "RECHAZADA",
            aprobada: data.resultado === 7? "S" : "N",
            rechazada: data.resultado === 6? "S" : "N",
            valorCredito: valorSolicitud?.valorCredito,
            plazoCredito: valorSolicitud?.plazo,
            teaCredito: valorSolicitud?.tasaEA,
            fechaCreacion: moment().format(),
            creadoPor: user.USR_ID,
        }
        crearComiteCreditoSolicitud(request);
        obtenerTramitesPendientes();
        handleCloseModal();
        formulario.reset();
        // onItemGoBack();
    }

    const crearComiteCreditoSolicitud = async (request: any) => {
        await apiCredit
            .postCreateCreditCommitteeRequest(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess && response.data) {
              
                        const result =  response.data?.resultado;
                        const arrayNuevo: any[] = [];
                        array.forEach((item) => {
                            arrayNuevo.push({
                                descripcionCredito: item.descripcionCredito,
                                identificacion: item.identificacion,
                                nombres: item.nombres,
                                numeroSolicitud: item.numeroSolicitud,
                                valorCredito: item.valorCredito,
                                plazo: item.plazo,
                                tasaEA: item.tasaEA,
                                resultado: result
                            });
                                    
                         });
                        setResultado(arrayNuevo);
                        await NotificationService.genericAlert(
                            'Guardado con éxito ',
                            'Se realizó  con éxito el cambio de estado de la solicitud',
                        );
                    }
                }
            })
            .catch((e) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleChangeApprovedRejected = (
        type: string,
        index: any,
        value: any,
    ) => {
        const inlineValuesAdapted = [...inlineValues];

        if (type === 'approved') {
            if (value === 'S') {
                inlineValuesAdapted[index].approved = 'S';
                inlineValuesAdapted[index].rejected = 'N';
           
            } else {
                inlineValuesAdapted[index].approved = 'N';
                inlineValuesAdapted[index].rejected = 'S';
            }
        } else if (type === 'rejected') {
            if (value === 'S') {
                inlineValuesAdapted[index].approved = 'N';
                inlineValuesAdapted[index].rejected = 'S';
            } else {
                inlineValuesAdapted[index].approved = 'S';
                inlineValuesAdapted[index].rejected = 'N';
            }
        }
        setInlineValues(inlineValuesAdapted);
    };

    const handleChangeResult = ( index: number, resultado: string, ) => {
        
        const inlineValuesAdapted = [...inlineValues];
        inlineValuesAdapted[index].result = resultado;
        setInlineValues(inlineValuesAdapted);
    };
    const handleChangeValorCredito = (index: number, valorCredito: string) => {
        const inlineValuesAdapted = [...inlineValues];
        inlineValuesAdapted[index].valorCredito = valorCredito;
        setInlineValues(inlineValuesAdapted);
    };
    const handleChangePlazoCredito = (index: number, plazoCredito: string) => {
        const inlineValuesAdapted = [...inlineValues];
        inlineValuesAdapted[index].plazoCredito = plazoCredito;
        setInlineValues(inlineValuesAdapted);
    };
    
    const handleChangeTeaCredito = (index: number, teaCredito: string,) => {
        const inlineValuesAdapted = [...inlineValues];
        inlineValuesAdapted[index].teaCredito = teaCredito;
        setInlineValues(inlineValuesAdapted);
    };
 
    if (isLoading) {
        return <CHLoadingProcess />;
    }

    const columns = [
       
        {
            name: 'descripcionCredito',
            label: 'Tipo de Crédito',
        },
        {
            name: 'numeroSolicitud',
            label: 'Número de Solicitud',
        },
        {
            name: 'identificacion',
            label: 'Cédula',
        },
        {
            name: 'nombres',
            label: 'Nombre Cliente',
        },
        {
            name: 'resultado',
            label: 'Resultado',
    
        },
        {
            name: 'valorCredito',
            label: 'Valor Crédito',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'plazo',
            label: 'Plazo Crédito',
          
        },
    
        mostrarBotonProcesar? 
        {
            name: 'action',
            label: 'Acción',
            options: {
                customBodyRender: (value: any, tableMeta: any) => (
                    <Button
                        color="primary"
                        variant="contained"
                        onClick={handleProcess(tableMeta.rowIndex)}
                    >
                        Procesar
                    </Button>
                ),
            },
        }: '',
     
    ];

    return (
        <>
            <CHPaper title="Solicitudes Crédito">
               
                <div
                    style={{
                        display: 'table',
                        tableLayout: 'fixed',
                        width: '100%',
                    }}
                >
                    <MUIDataTable
                        columns={columns}
                        data={resultado}
                        title=""
                        options={{
                            selectableRows: 'none',
                            responsive: 'vertical',
                            filter: false,
                            search: false,
                            download: false,
                            viewColumns: false,
                            print: false,
                            elevation: 0,
                            textLabels: {
                                body: {
                                    noMatch: 'Ningun Resultado',
                                },
                                pagination: {
                                    displayRows: 'de',
                                    next: 'Siguiente',
                                    previous: 'Anterior',
                                    rowsPerPage: 'Filas por página',
                                    jumpToPage: '',
                                },
                            
                              
                            },
                        }}
                    />
                    <br />
                     <Button onClick={onItemGoBack}
                    style={{ backgroundColor: '#1C3F8B', color:'white'}}

                >
                    Regresar
                </Button>
                <br />
                </div>
            </CHPaper>
           <ModalCreditCommitee
                open={openModal}
                handleClose={handleCloseModal}
                formulario={formulario}
                causalesRechazo={causalesRechazo}
                estadoSolicitud={estadoSolicitud}
                handleOnClick={handleOnClick}
            />
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    isLoading: state.credit.loading,
    list: state.credit.creditCommitteeRequestList,
    isSuccess: state.credit.approveCreditCommitteeSuccessfully,
});

const mapDispatchToProps = {
    startPostCreditCommitteeRequestSuccess,
    startPostCreateCreditCommitteeRequest,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(CreditCommitteeRequest);


