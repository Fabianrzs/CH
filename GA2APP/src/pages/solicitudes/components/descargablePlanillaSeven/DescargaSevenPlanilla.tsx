


import React, { useState } from "react";
import { Button } from "@material-ui/core";


const DescargaSevenPlanilla = (
    {
        obtenerPlanillaPdf
    }:
    {
        obtenerPlanillaPdf:any
    }) => {

    const [ejecutar, setEjecutar] = useState('')
    
    return(
        <>
            <Button
           variant="contained" color="primary"
                onClick={() =>  obtenerPlanillaPdf() }
            >Descarga Planilla Seven
            </Button>
        </>
     
    )


}

export default DescargaSevenPlanilla