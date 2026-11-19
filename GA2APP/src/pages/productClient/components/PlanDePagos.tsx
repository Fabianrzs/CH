import React, { useEffect } from 'react';
import TablaCreditoPlanPago from './TablaCreditoPlanPago';
import TablaLeasingPlanPago from './TablaLeasingPlanPago';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from "../../../services/auth.service";



function PlanDePagos  (props: any)  {
  const {   
    producto,clienteRequest
} = props;
useEffect(()=>{
  const auditoriaApi=AuditoriaApi();
  const user = AuthService.getUserInfo();
  const nav =(window.navigator as any);
  const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
  
  const requestAuditoria={
      navegador:usrAg,
      sistemaOperativo:nav.userAgentData.platform,
      nombreUsuario:user.USR_USERNAME, 
      usurioId:user.USR_ID,
      ip:'192.168.217.19',
      modulo:'Consulta de cartera',
      hostname:document.location.host
  } as Auditoria
  requestAuditoria.tipoAccion='CONSULTAS';
  requestAuditoria.idTipoAccion=3;
  requestAuditoria.descripcion=`Consultó pestaña Plan de Pagos en botón Detalle  parámetros:            
  \n Identificación cliente:  ${clienteRequest.identificacion}`
  auditoriaApi.InsertAuditoria(requestAuditoria);
})
  return (
    <div>
      
      
      {
        producto.idTipoCredito === 1 ?
          <TablaLeasingPlanPago
            producto={producto} 
            clienteRequest={clienteRequest}
          /> :
          <TablaCreditoPlanPago
            producto={producto} 
            clienteRequest={clienteRequest}
            />
      }
      
        
      
    </div>
  );
}

export default PlanDePagos;
