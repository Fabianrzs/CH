import React, { useEffect, useRef, useState } from 'react';
import { Button, CircularProgress, createStyles, Grid, makeStyles } from '@material-ui/core';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import moment from 'moment';
import CreditApi from '../../../services/api-credit.service';
import TablaListasPendientes from '../TablaListasPendientes';
import TableMassivePayAplicationList from './TableMassivePayAplicationList';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import CarteraApi from '../../../services/api-cartera-service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%'
        },
        buttonCompare: {
            // width: '100%',
        },
        groupButtonLoader: {
            display: 'flex',
            gap: '1.5rem',
            alignItems: 'center',
            padding: '1rem 0',
            flexWrap: 'wrap'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr 3fr',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        }
    }),
);


const ListasPendientes = (props: any) => {

    const creditApi = CreditApi();
    const carteraApi = CarteraApi();
    const [listados, setListados] = useState<any[]>([]);
    const [listaSeleccionada, setListaSeleccionada] = useState<any>(null);
    const [loading, setLoading] = useState(false);
    const [procesandoAplicacionPago, setProcesandoAplicacionPago] = useState<boolean>(false);
    const [procesandoEliminacionLista, setProcesandoEliminacionLista] = useState<boolean>(false);
    const [selectedRows, setSelectedRows] = useState<any[]|undefined>([]);

    const classes = useStyles();

    useEffect(
        () => {
            cargarListasPendientes();
        }, []
    );

    const handleSelectedRow = (__: any, _: any, rowsSelected: any) => {
        setSelectedRows(rowsSelected);
        const index = rowsSelected[0];
        if(index !== undefined) {
            setListaSeleccionada(listados[index]);
        } else {
            setListaSeleccionada(null);
        }

    }

    const cargarListasPendientes = () => {
        setLoading(true);
        creditApi.obtenerProcesoAplicacionPagos({estado: 'PENDIENTE'})
        .then(x => {
            if(x.isSuccess) {
                setListados(x.data ?? []);
            }
        })
        .catch(_ => setListados([]))
        .finally(() => setLoading(false));
    }


    const insertarPagoMasivoDesdeListaSV= () => {
        
        const request = {
            IdProceso : listaSeleccionada.idProceso,
            fechaPago : moment(listaSeleccionada.fecha_Pago).format()
        }
        setLoading(true);
        carteraApi
        .aplicarPagoMasivoDesdeListaSV(request)
        .then((response: any) => {
            if(response.isSuccess) {

                    toast(` ${response.returnMessage}`, { type: toast.TYPE.SUCCESS });
               
            }else {
                toast(` ${response.returnMessage}`, { type: toast.TYPE.ERROR, autoClose: 10000 });

            }
        })
        .catch((error) => {
            if (error.response && error.response.data ) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000 });
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
        }).finally(() => setLoading(false));
    }
      




    const handleAplicarPagos = async () => {

        if(listaSeleccionada?.idProceso) {

            const result = await swal({ text: '¿Está seguro que desea aplicar pagos?', buttons: { 
                confirm: { text: "Sí", value: true, visible: true },
                cancel: { text: "No", value: false, visible: true }
            }, icon: 'warning' });

            if(result) {
                setProcesandoAplicacionPago(true);
                creditApi.aplicarPagoMasivoDesdeLista(listaSeleccionada?.idProceso)
                .then(x => {
                    if(x.isSuccess) {
                        insertarPagoMasivoDesdeListaSV()
                        setListaSeleccionada(null);
                        setSelectedRows([]);
                        cargarListasPendientes();
                        swal({ text: 'Se han aplicado los pagos correctamente', icon: 'success'});
                    } else {
                        toast("Ha ocurrido un error al aplicar los pagos", { type: toast.TYPE.ERROR });
                    }
                })
                .catch(_ => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                 })
                .finally(() => setProcesandoAplicacionPago(false));
            }
            
        }
    }

    const handleEliminarProcesoAplicacionPago = async (idProceso: number) => {

        if(idProceso) {
            const result = await swal({ text: '¿Está seguro que desea eliminar la lista?', buttons: { 
                confirm: { text: "Sí", value: true, visible: true },
                cancel: { text: "No", value: false, visible: true }
            }, icon: 'warning' });

            if(result) {
                setProcesandoEliminacionLista(true);
                creditApi.eliminarProcesoAplicacionPagos(idProceso)
                .then(x => {
                    if(x.isSuccess) {
                        if(idProceso === listaSeleccionada?.idProceso) {
                            setListaSeleccionada(null);
                            setSelectedRows([]);
                        }
                        setListados(listados.filter((x: any) => x.idProceso !== idProceso));
                    } else {
                        toast(x?.returnMessage ?? "Ha ocurrido un error al eliminar la lista", { type: toast.TYPE.ERROR });
                    }
                })
                .catch(_ => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    })
                .finally(() => setProcesandoEliminacionLista(false));
            }
        }
    }

    const formatTotal = (value?: number) => {
        const formatter = new Intl.NumberFormat('en-EN', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
        });

        return formatter.format(value ?? 0);
    }

    if (loading) {
		return <CHLoadingProcess />;
	}

    const listInfo = {
        periodo: listaSeleccionada?.periodo ?? '',
        unidadEjecutora: listaSeleccionada?.unidadEjecutora ?? '',
        tipoCredito: listaSeleccionada?.tipoCredito ?? ''
    };

    return (<>

        <Grid container>
            <Grid item xs={12}>
                <TablaListasPendientes listState={listados} rowsSelected={selectedRows} 
                onDeleteProceso={handleEliminarProcesoAplicacionPago}
                onRowSelectionChange={handleSelectedRow} selectableRows='single'/>
            </Grid>
        </Grid>
        <br />
        {
            ((listaSeleccionada?.pagos?.length ?? 0) > 0) &&
            <Grid container>
                <Grid item xs={12}>
                    <TableMassivePayAplicationList 
                    listState={listaSeleccionada?.pagos ?? []}
                     listInfo={listInfo} 
                     selectableRows='none'
                     onDeletePago={() => {}}
                     />
                </Grid>
                <Grid item xs={12}>
                    <div className={classes.groupButtonLoader}>
                        <Button 
                        variant="contained" 
                        color="primary" 
                        component="label" 
                        onClick={handleAplicarPagos} 
                        disabled={procesandoAplicacionPago || procesandoEliminacionLista}
                        >
                                APLICAR PAGOS
                        </Button>
                        {/* <Button variant="contained" color="primary" component="label" onClick={insertarPagoMasivoDesdeListaSV} disabled={procesandoAplicacionPago || procesandoEliminacionLista}>
                                SEVEN
                        </Button> */}
                        <span style={{fontWeight: 'bold', fontSize: '18px'}}>Total: {formatTotal(listaSeleccionada?.total)}</span>
                        {
                            procesandoAplicacionPago && <CircularProgress  size='1.5rem' />
                        }
                    </div>
                </Grid>
            </Grid>
        }

    </>);
}

export default ListasPendientes;