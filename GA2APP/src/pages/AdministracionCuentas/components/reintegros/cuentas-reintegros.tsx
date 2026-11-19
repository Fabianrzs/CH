import {Card} from '@material-ui/core';
import React from 'react';
import { CuentaModelDto } from '../../../../models/requests/reintegros-request-request.model';
import ListaCuentasReintegro from './lista-cuentas-reintegro';

const CuentasReintegros = (props: props) => {
    const {cuentas, 
           tipoReintegro,
           estadoAfiliadoEnEvaluacion,
           listaConceptosSeleccionada,
           funcionAplicada,
           setListaConceptosSeleccionada,
          } = props;

    return(
        <>
           {cuentas.map((cuenta: CuentaModelDto) => (
                <Card
                    key={cuenta.cuentaId}
                    style={{ marginBottom: '15px', background: '#f1f2f6' }}
                >
                    <ListaCuentasReintegro
                      cuenta={cuenta}
                      tipoReintegro={tipoReintegro}
                      estadoAfiliadoEnEvaluacion={estadoAfiliadoEnEvaluacion}
                      listaConceptosSeleccionada={listaConceptosSeleccionada}
                      funcionAplicada={funcionAplicada}
                      setListaConceptosSeleccionada={setListaConceptosSeleccionada}
                    />
                </Card>
            ))}
        </>
    )
}

type props = {
    cuentas: CuentaModelDto[];
    tipoReintegro: number;
    estadoAfiliadoEnEvaluacion: boolean;
    listaConceptosSeleccionada: any[];
    funcionAplicada: number;
    setListaConceptosSeleccionada: (value: any[]) => void;
}

export default CuentasReintegros;