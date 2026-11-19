import React, { useEffect, useState } from 'react';
import { ProductoDto } from '../../../types/producto.model.type';
import { TABS_INDEX } from '../constants/producto.constants';
import ObligationsQuery from './ObligationsQuery';
import PayAplication from './PayAplication';
import ProductUpdates from './ProductUpdates';
import ProductQuery from './ProductQuery';
import { ClienteDto } from '../../../types/client.model.type';
import PlanDePagos from './PlanDePagos';

import { ProcedureRequestModel } from '../../../models/procedure-request.model';
import MostrarArchivos from './MostrarArchivos';
import CuotasAdelantadas from './CuotasAdelantadas';
import SaldosDiariosCartera from './SaldosDiariosCartera';
// import { REQUEST_ACTIVITY_TABS } from '../../../constants/constants';
// import { createStyles, withStyles, Theme, Tab } from '@material-ui/core';




const ProductPanel = (props: any) => {
    const {
       //  validarDocObligatorios,
        currentTabValue,
        idType,
        idNumber,
        productoRequest,
        clienteRequest,
        //   procedureRequest,
    }  
    = props;
   
    switch (currentTabValue) {
        case TABS_INDEX.CONSULTA_PRODUCTO:            
            return (
                <ProductQuery
                    productosSelec={productoRequest}
                    producto={productoRequest}
                    idType={idType}
                    idNumber={idNumber}
                    clienteRequest={clienteRequest}
                />
            );
        case TABS_INDEX.CONSULTA_OBLIGACIONES:            
            return (
                <ObligationsQuery
                    idType={idType}
                    idNumber={idNumber}
                    productosSelec={productoRequest}
                    clienteRequest={clienteRequest}
                />
            );
        case TABS_INDEX.APLICACION:           
            return <PayAplication
            productosSelec={productoRequest}
            idType={idType}
            idNumber={idNumber} 
            clienteRequest={clienteRequest}

            />;
        case TABS_INDEX.NOVEDADES:            
            return <ProductUpdates 
            producto={productoRequest}
            clienteRequest={clienteRequest}
            />;
 
        // case TABS_INDEX.APLICACION_MASIVA_DE_PAGOS:
        //     return <MassivePayAplication />;

        case TABS_INDEX.CUOTAS_ADELANTADAS:           
            return <CuotasAdelantadas producto={productoRequest} clienteRequest={clienteRequest}/>;

         case TABS_INDEX.PLAN_DE_PAGOS:           
            return <PlanDePagos 
            producto={productoRequest} clienteRequest={clienteRequest}/>;
         case TABS_INDEX.SALDOS_DIARIOS:           
            return <SaldosDiariosCartera 
            producto={productoRequest}
             clienteRequest={clienteRequest}/>;
        default:
            return null;

            case TABS_INDEX.UPLOAD_FILES:                
                return (
                    <MostrarArchivos
                        procedureRequest={productoRequest}
                        clienteRequest={clienteRequest}
                    />
    
                );

                // case REQUEST_ACTIVITY_TABS.UPLOAD_FILES:
                //     return (
                //         <AntTab
                //             value={TABS_INDEX.UPLOAD_FILES}
                //             key={`tab-${permission}`}
                //             label="Subir Archivos"
                //         />
                //     );


    }
};

type propsType = {
    currentTabValue: number;
    idType: number;
    idNumber: string;
    productoRequest: ProductoDto;
    clienteRequest: ClienteDto;
    procedureRequest: ProcedureRequestModel;
     validarDocObligatorios: any[];
    
};

export default ProductPanel;
