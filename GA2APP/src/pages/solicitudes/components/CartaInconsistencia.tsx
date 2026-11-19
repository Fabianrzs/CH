import { Button } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import ModalInconsitencia from "../ModalIncositencia";

interface Props {

    responseSolicitud: any;
    setDeshalitarBotonesInconsistencia: any;
    onGeneradoExitoso?: () => void;

}

const CartaInconsistencia = (props: Props) => {

    const {
        responseSolicitud,
        setDeshalitarBotonesInconsistencia,
        onGeneradoExitoso
    } = props;


    const [open, setOpen] = useState(false);

    const generarCartaInconsistecia = () => {
        setOpen(true)
    }
    const handleClose = () => {
        setOpen(false);
    };

    return (
        <>

            <div style={{ textAlign: 'center', width: '20%' }}>
                <Button style={{}} color="primary"
                    variant="contained"
                    onClick={() => generarCartaInconsistecia()}>
                    Carta Inconsistencia
                </Button>
            </div>

            <ModalInconsitencia
                open={open}
                onClose={handleClose}
                responseSolicitud={responseSolicitud}
                setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
                onGeneradoExitoso={onGeneradoExitoso}
            />
        </>
    )


}

export default CartaInconsistencia