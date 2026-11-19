
import React from 'react'
import { ResponseSeveranceModel } from '../../../../../models/requests/response-severance.model';

function AdquisicionViviendaNueva({
    onItemGoBack,
    tipoAccion,
    responseSolicitud,
    onItemBack,
}: {

    onItemGoBack: () => void;
    tipoAccion: number;
    responseSolicitud: ResponseSeveranceModel | undefined;
    onItemBack: () => void;

}) {

    return (
        <div>desde Adqusición vivienda nueva </div>
    )
}

export default AdquisicionViviendaNueva