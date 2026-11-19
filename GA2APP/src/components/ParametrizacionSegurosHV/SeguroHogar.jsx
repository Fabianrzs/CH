import { Button, Grid } from "@material-ui/core";
import MaterialTable from "material-table";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import CreditApi from "../../services/api-credit.service";
import CHPaper from "../ch-paper/CHPaper";
import ModalAgregarSeguroH from "./Modales/ModalAgregarSeguroH";
import TablasSeguros from "./TablaSeguros/TablasSeguros";

function SeguroHogar(params) {
   
    const apiCredito = CreditApi()
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [itemTabla, setItemTabla] = useState({});
    const [aseguradoras, setAseguradoras] = useState([]);
    const [itemActulizar, setItemActulizar] = useState({});

   
    useEffect(()=> {
        
        obtenerAseguradoras();
       
    },[itemTabla, setItemTabla])
    const handleOpenModalAgregar = () => {
        obtenerAseguradorasParaActulizar()
        setOpenModalAgregar(true);
    };
    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };


    const obtenerAseguradorasCreadas = (items) => {   
        setItemTabla(items)
    }

    const obtenerAseguradorasParaActulizar = (items) => {
        setOpenModalAgregar(true);
        setItemActulizar(items)
    }

    const obtenerAseguradoras = async () => {
        await apiCredito
            .getObtenerAseguradoras()
            .then(async (response) => {
                if (response) {
                  
                    if (response.isSuccess) {
                        setAseguradoras(response?.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    return(
       <>
       
        <CHPaper title="Parametrización Seguro">
        <Grid container>
                <Button
                    style={{marginTop:'20px'}}
                    size="small"
                    variant="contained"
                    color="primary"
                    onClick={handleOpenModalAgregar}
                >
                    AGREGAR NUEVO SEGURO
                </Button>
            </Grid>
            <ModalAgregarSeguroH
             open = { openModalAgregar }
             handleClose={handleCloseModalAgregar}
             obtenerAseguradorasCreadas = {obtenerAseguradorasCreadas}
             itemActulizar = { itemActulizar } />
         </CHPaper>
       <TablasSeguros 
            aseguradoras = {aseguradoras}
            obtenerAseguradorasParaActulizar = {obtenerAseguradorasParaActulizar}
       />
       </> 
    )
}

export default SeguroHogar;

