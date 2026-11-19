import React from 'react';
import { UseFormReturn } from 'react-hook-form';
import Entrevista from './Entrevista';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import IdentificacionPostulantesNucleoFamiliarPostulacion from './IdentificacionPostulantesNucleoFamiliarPostulacion';
import { CatalogState } from '../../../reducers/catalog.reducer';

const EntrevistaPostulacion = (props: propsType) => {

    const {datosSolicitudPostulacion,
           catalogState,
           formularioEntrevista, 
           postulantesControl,
           datosEntrevista,
           cliente,
           listaTiposIntegrantes,
           listaTiposAyudaTecnica,
           listaTiposDiscapacidad,
           camposEntrevistaSoloLectura,
           setListaPostulantes,
           consultarTiposIntegrantes,
           consultarTiposDiscapcidad,
           consultarTiposAyudaTecnica,
           } = props;

    return (
        <>
            <IdentificacionPostulantesNucleoFamiliarPostulacion 
                datosSolicitudPostulacion={datosSolicitudPostulacion}
                catalogState={catalogState} 
                datosEntrevista={datosEntrevista}
                cliente={cliente}
                listaTiposIntegrantes={listaTiposIntegrantes}
                listaTiposDiscapacidad={listaTiposDiscapacidad}
                listaTiposAyudaTecnica={listaTiposAyudaTecnica}
                camposEntrevistaSoloLectura={camposEntrevistaSoloLectura}
                postulantesControl={postulantesControl}
                setListaPostulantes={setListaPostulantes} 
                consultarTiposIntegrantes={consultarTiposIntegrantes}
                consultarTiposDiscapcidad={consultarTiposDiscapcidad}
                consultarTiposAyudaTecnica={consultarTiposAyudaTecnica}
            />
            <Entrevista
                formularioEntrevista={formularioEntrevista}
                camposEntrevistaSoloLectura={camposEntrevistaSoloLectura}
            />
        </>
    )
}

type propsType = {
    datosSolicitudPostulacion: any;
    catalogState: CatalogState;
    formularioEntrevista: UseFormReturn<any>;
    postulantesControl: UseFormReturn<any>;
    datosEntrevista: any;
    cliente: any;
    listaTiposIntegrantes: any;
    listaTiposAyudaTecnica: any;
    listaTiposDiscapacidad: any;
    camposEntrevistaSoloLectura: boolean;
    setListaPostulantes: (postulantes: Postulante[]) => void;
    consultarTiposIntegrantes: () => void;
    consultarTiposDiscapcidad: () => void;
    consultarTiposAyudaTecnica: () => void;
}

export default EntrevistaPostulacion;