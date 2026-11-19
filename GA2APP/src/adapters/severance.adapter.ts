import { isString } from 'lodash';
import { ProcedureRequestModel } from '../models/procedure-request.model';

class SeveranceAdapter {

    static adaptProcessRequestSeverance(rawList: any): ProcedureRequestModel {
        return SeveranceAdapter.adaptProcessRequest(rawList);
    }

    static adaptProcessRequest(rawItem: any): ProcedureRequestModel {
        const {
            tipO_CREDITO,
            fechA_SOLICITUD,
            numerO_SOLICITUD,
            cedulA_CLIENTE,
            nombrE_CLIENTE,
            actividad,
            orden,
            estadO_ACTIVIDAD,
            iD_SOLICITUD,
            iD_SIMULACION,
            iD_ESTADO_ACTIVIDAD,
            usuariO_RESPONSABLE,
            iD_TIPO_CREDITO,
            conveniO_ASEGURADORA,
            codigO_ASEGURADORA,
            aseguradora,
            nO_CELULAR,
            correO_PERSONAL,
            direccioN_RESIDENCIA,
            telefonO_RESIDENCIA,
            tipO_INMUEBLE,
            tipO_VIVIENDA,
            valoR_INMUEBLE,
            valoR_CREDITO,
            plazO_FINANCIEACION,
            porC_FINANCIACION,
            capturA_DATOS,
            actividaD_PRINCIPAL,
            iD_ACTIVIDAD_FLUJO,
            cargA_ARCHIVO,
            visualizaR_ARCHIVOS,
            puedE_DELEGAR,
            enviO_NITICACION,
            paneles,
            enviO_NOTICIACION_VENC,
            coloR_GRILLA,
            trS_ID,
            generA_PDF_RESUMEN,
            iD_CUENTA,
            cLI_ID,
            desistiR_FLUJO
        } = rawItem || {};
        return {
            activity: actividad,
            canDelegate: puedE_DELEGAR,
            cellphoneNumber: nO_CELULAR,
            clientIdentificationNumber: cedulA_CLIENTE,
            clientName: nombrE_CLIENTE,
            creditType: tipO_CREDITO,
            creditTypeID: iD_TIPO_CREDITO,
            creditValue: valoR_CREDITO,
            dataCaptured: capturA_DATOS,
            dealInsurance: conveniO_ASEGURADORA,
            fileLoad: cargA_ARCHIVO,
            financingPercentage: porC_FINANCIACION,
            financingTerm: plazO_FINANCIEACION,
            homeAddress: direccioN_RESIDENCIA,
            homePhone: telefonO_RESIDENCIA,
            insurance: aseguradora,
            insuranceCode: codigO_ASEGURADORA,
            livingType: tipO_VIVIENDA,
            notificationSend: enviO_NITICACION,
            notificationSendExpiration: enviO_NOTICIACION_VENC,
            order: orden,
            personalEmail: correO_PERSONAL,
            principalActivity: actividaD_PRINCIPAL,
            propertyType: tipO_INMUEBLE,
            propertyValue: valoR_INMUEBLE,
            requestDate: fechA_SOLICITUD,
            requestID: iD_SOLICITUD,
            requestNumber: numerO_SOLICITUD,
            responsibleUser: usuariO_RESPONSABLE,
            simulationID: iD_SIMULACION,
            stateActivity: estadO_ACTIVIDAD,
            stateActivityID: iD_ESTADO_ACTIVIDAD,
            tableColor: coloR_GRILLA,
            typeCreditID: iD_TIPO_CREDITO,
            visualizeFiles: visualizaR_ARCHIVOS,
            trsID: trS_ID,
            flowActivityID: iD_ACTIVIDAD_FLUJO,
            panels: SeveranceAdapter.adaptPanels(paneles),
            generateResumePDF: generA_PDF_RESUMEN === 'S',
            idCuenta: iD_CUENTA,
            clienteId: cLI_ID,
            desistiR_FLUJO: desistiR_FLUJO
        };
    }

    static adaptPanels(rawString: any): string[] {
        const adaptedPanels: string[] = [];
        if (isString(rawString)) {
            let stringAdapted = rawString;
            stringAdapted = stringAdapted.replace('[', '');
            stringAdapted = stringAdapted.replace(']', '');
            stringAdapted.split(',').forEach(val => {
                adaptedPanels.push(val);
            });
        }
        return adaptedPanels;
    }
}

export default SeveranceAdapter;
