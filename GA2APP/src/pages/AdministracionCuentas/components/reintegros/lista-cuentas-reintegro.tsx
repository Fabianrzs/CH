import React, { useEffect, useState } from 'react';
import { IconButton } from '@material-ui/core';
import { AddCircle } from '@material-ui/icons';
import UtilService from '../../../../utils/util.service';
import { CuentaModelDto } from '../../../../models/requests/reintegros-request-request.model';
import ListaConceptosReintegros from './lista-conceptos-reintegro';

const ListaCuentasReintegro = (props: props) => {

    const {cuenta,
           tipoReintegro,
           estadoAfiliadoEnEvaluacion,
           listaConceptosSeleccionada,
           funcionAplicada,
           setListaConceptosSeleccionada} = props;

    const [showConcept, setShowConcept] = useState(false);
    const [deshabilitarLista, setDesahibilitarLista] = useState(false);

    useEffect(() => {
        if(cuenta){
            if(cuenta?.estadoCuenta === 'CANCELADA' && cuenta?.saldo === 0){
               setDesahibilitarLista(true);
            }else if(cuenta?.saldo === 0){
               setDesahibilitarLista(true);
            }else if(cuenta?.conceptos.length === 0){
               setDesahibilitarLista(true);
            }else{
               setDesahibilitarLista(false);
            }
        }
    },[])

    const handleClick = () => setShowConcept(!showConcept);

    return (
        <>
             <div>
                <span style={{ marginLeft: '15px' }}>
                    <b> CUENTA: </b>
                </span>
                {cuenta.nombreCuenta}
                <span>
                    <b> NÚMERO CUENTA: </b>
                </span>
                {cuenta.numeroCuenta}
                <span>
                    <b> SALDO: </b>
                </span>
                {UtilService.formatCurrency(cuenta.saldo)}
                    <IconButton onClick={handleClick} disabled={deshabilitarLista}>
                        <AddCircle /> 
                    </IconButton>
            </div>
            <div>
                {showConcept ? (
                  <ListaConceptosReintegros 
                    conceptos={cuenta.conceptos}
                    cuentaId={cuenta.cuentaId}
                    tipoReintegro={tipoReintegro}
                    estadoAfiliadoEnEvaluacion={estadoAfiliadoEnEvaluacion}
                    listaConceptosSeleccionada={listaConceptosSeleccionada}
                    funcionAplicada={funcionAplicada}
                    setListaConceptosSeleccionada={setListaConceptosSeleccionada}
                  />
                ) : null}
            </div>
        </>
    );
};

type props = {
    cuenta: CuentaModelDto;
    tipoReintegro: number;
    estadoAfiliadoEnEvaluacion: boolean;
    listaConceptosSeleccionada: any[];
    funcionAplicada: number;
    setListaConceptosSeleccionada: (value: any[]) => void;
}

export default ListaCuentasReintegro;