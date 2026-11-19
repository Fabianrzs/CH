import { isArray, isString } from 'lodash';
import { ProcedureRequestModel } from '../models/procedure-request.model';
import { CreditBasicDataModel } from '../models/credit-basic-data.model';
import { CreditProductModel } from '../models/credit-product.model';
import { CreditObservationModel } from '../models/credit-observation.model';
import { CreditFileModel } from '../models/credit-file.model';
import { CreditFinancialDataModel } from '../models/credit-financial-data.model';
import {
    CreditTechnicalDataModel,
    CreditTechnicalPropertyInformationData,
} from '../models/credit-technical-data.model';
import {
    CreditLegalDataModel,
    CreditLegalPropertyDataModel,
} from '../models/credit-legal-data.model';
import { CreditFlowDocumentModel } from '../models/credit-flow-document.model';
import { CreditSimulationDataModel } from '../models/credit-simulation-data.model';
import { CreditConsultCreditModel } from '../models/credit-consult-credit.model';
import { CreditCreateSimulationDataModel } from '../models/credit-create-simulation-data.model';
import { CarteraComprobanrtesDataModel, lstMovimientoComprobante } from '../models/cartera-comprobantes.model';

class CreditAdapter {

    static adaptProcessRequestSeverance(rawList: any): ProcedureRequestModel {
        return CreditAdapter.adaptProcessRequest(rawList);
    }

    static adaptProcessRequestList(rawList: any[]): ProcedureRequestModel[] {
        const processRequestList: ProcedureRequestModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                processRequestList.push(
                    CreditAdapter.adaptProcessRequest(item),
                );
            });
        }

        return processRequestList;
    }

    static adaptProcessRequest(rawItem: any): any {
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
            rechazA_FLUJO,
            devuelvE_FLUJO,
            retornA_ACTIVIDAD_ID,
            desistiR_FLUJO,
            idActivity,
            iD_ACTIVIDAD,
            numerO_RADICADO,
            sdP_OPCION_COMPRA_CARTERA,
            observacioN_ACTIVIDAD,
            edad,
            valorPrestamo,
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
            panels: CreditAdapter.adaptPanels(paneles),
            generateResumePDF: generA_PDF_RESUMEN === 'S',
            rechazaFlujo: rechazA_FLUJO,
            devuelveFlujo: devuelvE_FLUJO,
            retornoActividad: retornA_ACTIVIDAD_ID,
            desistiR_FLUJO: desistiR_FLUJO,
            idActivity: iD_ACTIVIDAD,
            numerO_RADICADO: numerO_RADICADO,
            sdP_OPCION_COMPRA_CARTERA: sdP_OPCION_COMPRA_CARTERA,
            observacioN_ACTIVIDAD: observacioN_ACTIVIDAD,
            edad:edad,
            valorPrestamo:valorPrestamo

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

    static adaptCreditBasicData(rawItem: any): CreditBasicDataModel {
        const {
            soB_ID,
            soC_ID,
            tiD_ID,
            soB_NUMERO_DOCUMENTO,
            soB_FECHA_EXPEDICION,
            dpD_ID_EXP,
            dpC_ID_EXP,
            soB_PRIMER_APELLIDO,
            soB_SEGUNDO_APELLIDO,
            soB_PRIMER_NOMBRE,
            soB_SEGUNDO_NOMBRE,
            soB_FECHA_NACIMIENTO,
            dpD_ID_NACIMIENTO,
            dpC_ID_NACIMIENTO,
            soB_SEXO,
            soB_ESTADO_CIVIL,
            soB_NIVEL_EDUCACION,
            frZ_ID,
            ctG_ID,
            grD_ID,
            dpD_ID_RESIDENCIA,
            dpC_ID_RESIDENCIA,
            soB_DIRECCION_RESIDENCIA,
            soB_CORREO_PERSONAL,
            soB_TELEFONO_RESIDENCIA,
            soB_CELULAR,
            dpD_ID_OFICINA,
            dpC_ID_OFICINA,
            soB_DIRECCION_OFICINA,
            soB_CORREO_INSTITUCIONAL,
            soB_TELEFONO_OFICINA,
            acC_ID,
            prF_ID,
            soB_TIENE_TRANSACCION_ME,
            soB_TIPO_TRANSACCION_ME,
            soB_TIPO_PRODUCTO_ME,
            soB_BANCO_TRANSACCION_ME,
            soB_NUMERO_PRODUCTO_ME,
            soB_SALDO_PROMEDIO_CUENTAS_ME,
            soB_MONEDA_TRANSACCION_ME,
            dpP_ID_ME,
            dpD_ID_ME,
            soB_CREADO_POR,
            soB_ACTUALIZADO_POR,
            soB_FECHA_CREACION,
            soB_FECHA_ACTUALIZA,
            departamentO_EXPEDICION,
            ciudaD_EXPEDICION,
            departamentO_NACIMIENTO,
            ciudaD_NACIMIENTO,
            fuerza,
            categoria,
            grado,
            sexo,
            niveL_EDUCATIVO,
            estadO_CIVIL,
            profesion,
            tipO_DOCUMENTO_CLIENTE,
            

        } = rawItem || {};
        return {
            accID: acC_ID,
            bornCity: ciudaD_NACIMIENTO,
            bornState: departamentO_NACIMIENTO,
            category: categoria,
            civilState: estadO_CIVIL,
            ctgID: ctG_ID,
            dpcBirthID: dpC_ID_NACIMIENTO,
            dpcExpirationID: dpC_ID_EXP,
            dpcHomeID: dpC_ID_RESIDENCIA,
            dpcOfficeID: dpC_ID_OFICINA,
            dpdBirthID: dpD_ID_NACIMIENTO,
            dpdExpirationID: dpD_ID_EXP,
            dpdHomeID: dpD_ID_RESIDENCIA,
            dpdIDMe: dpD_ID_ME,
            dpdOfficeID: dpD_ID_OFICINA,
            dppIDMe: dpP_ID_ME,
            educationLevel: niveL_EDUCATIVO,
            expeditionCity: ciudaD_EXPEDICION,
            expeditionState: departamentO_EXPEDICION,
            force: fuerza,
            frzID: frZ_ID,
            grade: grado,
            grdID: grD_ID,
            prfID: prF_ID,
            profession: profesion,
            sex: sexo,
            sobAverageAccountBalanceME: soB_SALDO_PROMEDIO_CUENTAS_ME,
            sobBirthDate: soB_FECHA_NACIMIENTO,
            sobCellphone: soB_CELULAR,
            sobCivilStatus: soB_ESTADO_CIVIL,
            sobCreatedDate: soB_FECHA_CREACION,
            sobCreatedFor: soB_CREADO_POR,
            sobEducationLevel: soB_NIVEL_EDUCACION,
            sobExpeditionDate: soB_FECHA_EXPEDICION,
            sobFirstLastName: soB_PRIMER_APELLIDO,
            sobFirstName: soB_PRIMER_NOMBRE,
            sobHasTransactionME: soB_TIENE_TRANSACCION_ME,
            sobHomeAddress: soB_DIRECCION_RESIDENCIA,
            sobHomePhone: soB_TELEFONO_RESIDENCIA,
            sobID: soB_ID,
            sobIDNumber: soB_NUMERO_DOCUMENTO,
            sobMiddleName: soB_SEGUNDO_NOMBRE,
            sobOfficeAddress: soB_DIRECCION_OFICINA,
            sobOfficeEmail: soB_CORREO_INSTITUCIONAL,
            sobOfficePhone: soB_TELEFONO_OFICINA,
            sobPersonalEmail: soB_CORREO_PERSONAL,
            sobProductNumberME: soB_NUMERO_PRODUCTO_ME,
            sobProductTypeME: soB_TIPO_PRODUCTO_ME,
            sobSecondLastName: soB_SEGUNDO_APELLIDO,
            sobSex: soB_SEXO,
            sobTransactionBankME: soB_BANCO_TRANSACCION_ME,
            sobTransactionCurrencyME: soB_MONEDA_TRANSACCION_ME,
            sobTransactionTypeME: soB_TIPO_TRANSACCION_ME,
            sobUpdatedDate: soB_FECHA_ACTUALIZA,
            sobUpdatedFor: soB_ACTUALIZADO_POR,
            socID: soC_ID,
            tidID: tiD_ID,
            clientIdentificationType: tipO_DOCUMENTO_CLIENTE,
         

        };
    }

    static adaptCreditProductData(rawItem: any): CreditProductModel {
        const {
            soP_ID,
            soC_ID,
            soP_ESTADO_INMUEBLE,
            soP_TIPO_INMUEBLE,
            soP_TIENE_GARAJE,
            soP_TIENE_DEPOSITO,
            soP_CONJUNTO_CERRADO,
            tvV_ID,
            soP_AÑOS_CONSTRUCCION,
            soP_MATRICULA_INMOBILIARIA1,
            soP_MATRICULA_INMOBILIARIA2,
            soP_MATRICULA_INMOBILIARIA3,
            soP_VALOR_INMUEBLE,
            soP_DIRECCION_INMUEBLE,
            dpD_ID,
            dpC_ID,
            soP_VALOR_CREDITO,
            soP_PORC_FINANCIACION,
            soP_PLAZO_FINANCIACION,
            tiD_ID,
            soP_NUMERO_DOCUMENTO_CONSTRUCTOR,
            soP_FECHA_ENTREGA_INMUEBLE,
            soP_NOMBRE_FIDUCIA,
            soP_NOMBRE_PROYECTO,
            tiD_ID_VENDEDOR,
            soP_NUMERO_DOCUMENTO_VENDEDOR,
            soP_NOMBRE_VENDEDOR,
            soP_DIRECCION_VENDEDOR,
            dpD_ID_VENDEDOR,
            dpC_ID_VENDEDOR,
            soP_CORREO_VENDEDOR,
            soP_TELEFONO_VENDEDOR,
            soP_CREADO_POR,
            soP_FECHA_CREACION,
            soP_ACTUALIZADO_POR,
            soP_FECHA_ACTUALIZA,
            departamentO_INMUEBLE,
            ciudaD_INMUEBLE,
            tipO_VIVIENDA,
            tipO_INMUEBLE,
            tipO_DOCUMENTO_VENDEDOR,
            nombrE_TIPO_VIVIENDA,
            soP_BARRIO_INMUEBLE,
            soP_LOCALIDAD_INMUEBLE,
            soP_PROPIEDAD_HORIZONTAL,
            soP_TIPO_DEPOSITO,
            soP_TIPO_GARAGE,
            canoN_INICIAL,
            fechaDesembolso,
            cuotA_TOTAL,
            cuotA_TOTAL_SIN_SEGUROS,
            tasA_ANUAL,
            tasA_MENSUAL,
            numerO_PREAPROBADO,
            porcentajE_FINANCIACION,
        } = rawItem || {};

        return {
            dpcID: dpC_ID,
            dpcSellerID: dpC_ID_VENDEDOR,
            dpdID: dpD_ID,
            dpdSellerID: dpD_ID_VENDEDOR,
            propertyCity: ciudaD_INMUEBLE,
            propertyState: departamentO_INMUEBLE,
            propertyType: tipO_VIVIENDA,
            socID: soC_ID,
            sopBuilderIDNumber: soP_NUMERO_DOCUMENTO_CONSTRUCTOR,
            sopClosedSet: soP_CONJUNTO_CERRADO,
            sopConstructionYears: soP_AÑOS_CONSTRUCCION,
            sopCreatedDate: soP_FECHA_CREACION,
            sopCreatedFor: soP_CREADO_POR,
            sopCreditValue: soP_VALOR_CREDITO,
            sopDeliverDate: soP_FECHA_ENTREGA_INMUEBLE,
            sopFinancingPercentage: soP_PORC_FINANCIACION,
            sopFinancingTerm: soP_PLAZO_FINANCIACION,
            sopHasDeposit: soP_TIENE_DEPOSITO,
            sopHasGarage: soP_TIENE_GARAJE,
            sopID: soP_ID,
            sopProjectName: soP_NOMBRE_PROYECTO,
            sopPropertyAddress: soP_DIRECCION_INMUEBLE,
            sopPropertyState: soP_ESTADO_INMUEBLE,
            sopTipoInmuble: tipO_INMUEBLE,
            sopPropertyType: soP_TIPO_INMUEBLE,
            sopPropertyValue: soP_VALOR_INMUEBLE,
            sopRealStateRegistration1: soP_MATRICULA_INMOBILIARIA1,
            sopRealStateRegistration2: soP_MATRICULA_INMOBILIARIA2,
            sopRealStateRegistration3: soP_MATRICULA_INMOBILIARIA3,
            sopSellerAddress: soP_DIRECCION_VENDEDOR,
            sopSellerEmail: soP_CORREO_VENDEDOR,
            sopSellerIDNumber: soP_NUMERO_DOCUMENTO_VENDEDOR,
            sopSellerName: soP_NOMBRE_VENDEDOR,
            sopSellerPhone: soP_TELEFONO_VENDEDOR,
            sopTrustName: soP_NOMBRE_FIDUCIA,
            sopUpdatedDate: soP_FECHA_ACTUALIZA,
            sopUpdatedFor: soP_ACTUALIZADO_POR,
            tidID: tiD_ID,
            tidSellerID: tiD_ID_VENDEDOR,
            tvvID: tvV_ID,
            sellerIdentificationType: tipO_DOCUMENTO_VENDEDOR,
            nombreTipoVivienda: nombrE_TIPO_VIVIENDA,
            barrioInmueble: soP_BARRIO_INMUEBLE,
            localidadInmueble: soP_LOCALIDAD_INMUEBLE,
            propiedadHorizontal:soP_PROPIEDAD_HORIZONTAL,
            tipoDeDeposito: soP_TIPO_DEPOSITO,
            tipoDeGaraje: soP_TIPO_GARAGE,
            canonInicial: canoN_INICIAL,
            fechDesembolso: fechaDesembolso,
            valorCuotaTotal: cuotA_TOTAL,
            cuotaTotalSinSeguros: cuotA_TOTAL_SIN_SEGUROS,
            tasaAnual: tasA_ANUAL,
            tasaMensual: tasA_MENSUAL,
            numeroPreaprobado: numerO_PREAPROBADO,
            porcentajeFinanciacion: porcentajE_FINANCIACION,
        };
    }

    static adaptObservationsList(rawList: any[]): CreditObservationModel[] {
        const observationsList: CreditObservationModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                observationsList.push(CreditAdapter.adaptObservation(item));
            });
        }

        return observationsList;
    }

    static adaptObservation(rawItem: any): CreditObservationModel {
        const {
            iD_TRAMITE,
            iD_ACTIVIDAD_TRAMITE,
            iD_ACTIVIDAD,
            actividad,
            descripcion,
            observacion,
            fechA_INGRESO,
            usuariO_INGRESO,
        } = rawItem || {};
        return {
            userName: usuariO_INGRESO,
            activity: actividad,
            activityID: iD_ACTIVIDAD,
            entryDate: fechA_INGRESO,
            descripcion: descripcion,
            observation: observacion,
            requestActivityID: iD_ACTIVIDAD_TRAMITE,
            requestID: iD_TRAMITE,
        };
    }

    static adaptFilesList(rawList: any[]): CreditFileModel[] {
        const filesList: CreditFileModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                filesList.push(CreditAdapter.adaptFile(item));
            });
        }

        return filesList;
    }

    static adaptFile(rawItem: any): CreditFileModel {
        const { tdC_ID, iD_ACTIVIDAD_FLUJO, actividad, tipO_DOCUMENTO, rutA_IMAGEN } =
            rawItem || {};
        return {
            docID: tdC_ID,
            activity: actividad,
            activityFlowID: iD_ACTIVIDAD_FLUJO,
            docType: tipO_DOCUMENTO,
            imagePath: rutA_IMAGEN,
        };
    }

    static adaptFinancialData(rawItem: any): CreditFinancialDataModel {
        const {
            soF_ID,
            soC_ID,
            soF_CONCEPTO_SALARIO,
            soF_VALOR_CONCEPTO_SALARIO,
            soF_CONCEPTO_1,
            soF_VALOR_CONCEPTO_1,
            soF_CONCEPTO_2,
            soF_VALOR_CONCEPTO_2,
            soF_CONCEPTO_3,
            soF_VALOR_CONCEPTO_3,
            soF_CONCEPTO_4,
            soF_VALOR_CONCEPTO_4,
            soF_CONCEPTO_5,
            soF_VALOR_CONCEPTO_5,
            soF_CONCEPTO_6,
            soF_VALOR_CONCEPTO_6,
            soF_VALOR_TOTAL_INGRESOS,
            soF_VALOR_TOTAL_EGRESOS,
            soF_DIRECCION_VIVIENDA,
            soF_MATRICULA_VIVIENDA,
            soF_VALOR_COMERCIAL_VIVIENDA,
            soF_MARCA_VEHICULO,
            soF_PLACA_VEHICULO,
            soF_VALOR_COMERCIAL_VEHICULO,
            soF_CREADO_POR,
            soF_FECHA_CREACION,
            soF_ACTUALIZADO_POR,
            soF_FECHA_ACTUALIZA,
        } = rawItem || {};

        return {
            socID: soC_ID,
            sofConcept1: soF_CONCEPTO_1,
            sofConcept1Value: soF_VALOR_CONCEPTO_1,
            sofConcept2: soF_CONCEPTO_2,
            sofConcept2Value: soF_VALOR_CONCEPTO_2,
            sofConcept3: soF_CONCEPTO_3,
            sofConcept3Value: soF_VALOR_CONCEPTO_3,
            sofConcept4: soF_CONCEPTO_4,
            sofConcept4Value: soF_VALOR_CONCEPTO_4,
            sofConcept5: soF_CONCEPTO_5,
            sofConcept5Value: soF_VALOR_CONCEPTO_5,
            sofConcept6: soF_CONCEPTO_6,
            sofConcept6Value: soF_VALOR_CONCEPTO_6,
            sofConceptSalary: soF_CONCEPTO_SALARIO,
            sofConceptSalaryValue: soF_VALOR_CONCEPTO_SALARIO,
            sofCreatedDate: soF_FECHA_CREACION,
            sofCreatedFor: soF_CREADO_POR,
            sofHomeAddress: soF_DIRECCION_VIVIENDA,
            sofHomeCommercialValue: soF_VALOR_COMERCIAL_VIVIENDA,
            sofHomeRegistration: soF_MATRICULA_VIVIENDA,
            sofID: soF_ID,
            sofTotalExpensesValue: soF_VALOR_TOTAL_EGRESOS,
            sofTotalRevenueValue: soF_VALOR_TOTAL_INGRESOS,
            sofUpdatedDate: soF_FECHA_ACTUALIZA,
            sofUpdatedFor: soF_ACTUALIZADO_POR,
            sofVehicleBrand: soF_MARCA_VEHICULO,
            sofVehicleCommercialValue: soF_VALOR_COMERCIAL_VEHICULO,
            sofVehiclePlate: soF_PLACA_VEHICULO,
        };
    }

    static adaptCreditTechnicalData(rawItem: any): CreditTechnicalDataModel {
        const {
            idSolicitudInformacionTecnica,
            idSolicitudCredito,
            valorAvaluoComercial,
            valorAvaluoCatastral,
            valorVentaInmueble,
            consideracionesFinales,
            declaraCumplimiento,
            condicionesSalvedades,
            conceptoEstTecnico,
            creadoPor,
            fechaCreacion,
            nombreAliado,
            estrato,
            solicitudesCreditoInfTecnicaInmueble,
        } = rawItem || {};
        return {
            commercialValue: valorAvaluoComercial,
            catastralValue: valorAvaluoCatastral,
            complianceDeclaration: declaraCumplimiento,
            conceptValue: conceptoEstTecnico,
            conditionsSavings: condicionesSalvedades,
            creditRequestID: idSolicitudCredito,
            creditTechnicalPropertyInformationData:
                CreditAdapter.adaptCreditTechnicalPropertyInformationList(
                    solicitudesCreditoInfTecnicaInmueble,
                ),
            finalConsiderations: consideracionesFinales,
            propertySaleValue: valorVentaInmueble,
            requestTechnicalInformationID: idSolicitudInformacionTecnica,
            createdDate: fechaCreacion,
            createdFor: creadoPor,
            createdName: nombreAliado,
            stratum: estrato,
        };
    }

    static adaptCreditTechnicalPropertyInformationList(
        rawList: any[],
    ): CreditTechnicalPropertyInformationData[] {
        const informationList: CreditTechnicalPropertyInformationData[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                informationList.push(
                    CreditAdapter.adaptCreditTechnicalPropertyInformation(item),
                );
            });
        }

        return informationList;
    }

    static adaptCreditTechnicalPropertyInformation(
        rawItem: any,
    ): CreditTechnicalPropertyInformationData {
        const {
            idSolicitudInfoTecnicaInmueble,
            idSolicitudtecnica,
            idSolicitudCredito,
            numeroMatricula,
            fechaMatricula,
            numeroChip,
            cedulaCatastral,
            creadoPor,
            fechaCreacion,
        } = rawItem || {};
        return {
            cadastralID: cedulaCatastral,
            creditRequestID: idSolicitudCredito,
            propertyChip: numeroChip,
            realStateRegistrationDate: fechaMatricula,
            realStateRegistrationNumber: numeroMatricula,
            requestPropertyTechnicalInformationID:
                idSolicitudInfoTecnicaInmueble,
            requestTechnicalInformationID: idSolicitudtecnica,
            createdDate: fechaCreacion,
            createdFor: creadoPor,
        };
    }

    static adaptCarteraComprobanrtesData(rawItem: any): CarteraComprobanrtesDataModel {
        const {
            aprobado,
            comprobante_Id,
            comprobante_Seven_Id,
            estado,
            fecha_Comprobante,
            fecha_Transaccion,
            lstMovimientoComprobante,
            tipo_Comprobante,
            tipo_Operacion,
            detalle_Movimiento_Id,
            total_Comprobante_Credito,
            total_Comprobante_Debito,
        } = rawItem || {};
        return {
            aprobado,
            comprobante_Id,
            comprobante_Seven_Id,
            estado,
            fecha_Comprobante,
            fecha_Transaccion,
            lstMovimientoComprobante:
            CreditAdapter.adaptCarteralstMovimientoComprobanteDataList(
                lstMovimientoComprobante,
            ),

            tipo_Comprobante,
            tipo_Operacion,
            detalle_Movimiento_Id:detalle_Movimiento_Id,
            total_Comprobante_Credito:total_Comprobante_Credito,
            total_Comprobante_Debito: total_Comprobante_Debito
            
        };
    }

    static adaptCarteralstMovimientoComprobanteDataList(
        rawList: any[],
    ): lstMovimientoComprobante[] {
        const lstMovimientoComprobante: lstMovimientoComprobante[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                lstMovimientoComprobante.push(
                    CreditAdapter.adaptCarteralstMovimientoComprobanteData(item),
                );
            });
        }

        return lstMovimientoComprobante;
    }

    static adaptCarteralstMovimientoComprobanteData(rawItem: any): lstMovimientoComprobante {
        const {
            centro_Costo,
            comprobante_CarteraId,
            credito,
            cuenta_Contable,
            debito,
            descripcion_Cuenta,
            detalle,
            movimiento_Comprobante_CarteraId,
            total_Registros,
        } = rawItem || {};
        return {
            centro_Costo:centro_Costo,
            comprobante_CarteraId:comprobante_CarteraId,
            credito:credito,
            cuenta_Contable:cuenta_Contable,
            debito:debito,
            descripcion_Cuenta:descripcion_Cuenta,
            detalle:detalle,
            movimiento_Comprobante_CarteraId:movimiento_Comprobante_CarteraId,
            total_Registros:total_Registros
        };
    }



    static adaptCreditLegalData(rawItem: any): CreditLegalDataModel {
        const {
            analisisPazySalvo,
            analisisRegPropHorizontal,
            analisisSitJuridica,
            analisisUlt20Años,
            analisisVendedor,
            conceptoEstJuridicio,
            conceptoJuridicoFin,
            creadoPor,
            edadJuridica,
            fechaCreacion,
            fechaPrimerActo,
            idSolicitudCredito,
            idSolicitudInformacionJuridica,
            linderos,
            recomendaciones,
            solicitudesCreditoInfoJurInmueble,
            tipoParqueadero,
            tradicionInmueble,
            viabilidadJurNegocio,
            direccion,
            codigoDepartamento,
            codigoCiudad,
            nombreAliado,
            ciudad,
            departamento,
            escritura,
            fechaEscritura,
            notaria,
            ciudadNotaria ,
            departamentoNotaria,
            descripcionInmueble,
          
        } = rawItem || {};
        return {
            analysisPeaceAndSave: analisisPazySalvo,
            analysisRegHorizontalProperty: analisisRegPropHorizontal,
            analysisSealer: analisisVendedor,
            analysisSitLegal: analisisSitJuridica,
            analysisUlt20Years: analisisUlt20Años,
            boundaries: linderos,
            createDate: fechaCreacion,
            createdFor: creadoPor,
            creditLegalPropertyData:
                CreditAdapter.adaptCreditLegalPropertyDataList(
                    solicitudesCreditoInfoJurInmueble,
                ),
            dateFirstAct: fechaPrimerActo,
            legalAge: edadJuridica,
            legalBusinessViability: viabilidadJurNegocio,
            legalConceptEnd: conceptoJuridicoFin,
            legalConceptEst: conceptoEstJuridicio,
            parkingType: tipoParqueadero,
            propertyTradition: tradicionInmueble,
            recommendations: recomendaciones,
            requestCreditID: idSolicitudCredito,
            requestInformationLegalID: idSolicitudInformacionJuridica,
            propertyAddress: direccion,
            propertyCityID: codigoCiudad,
            propertyStateID: codigoDepartamento,
            createdName: nombreAliado,
            propertyCity: ciudad,
            propertyState: departamento,
            escritura,
            fechaEscritura,
            notaria: notaria,
            ciudadNotaria: ciudadNotaria,
            departamentoNotaria: departamentoNotaria,
            descripcionInmueble: descripcionInmueble,
        };
    }

    static adaptCreditLegalPropertyDataList(
        rawList: any[],
    ): CreditLegalPropertyDataModel[] {
        const informationList: CreditLegalPropertyDataModel[] = [];
        if (isArray(rawList)) {
            rawList.forEach(item => {
                informationList.push(
                    CreditAdapter.adaptCreditLegalPropertyDataItem(item),
                );
            });
        }
        return informationList;
    }

    static adaptCreditLegalPropertyDataItem(
        rawItem: any,
    ): CreditLegalPropertyDataModel {
        const {
            areaPrivada,
            avaluoCatastral,
            avaluoComercial,
            cedulaCatastral,
            creadoPor,
            fechaCreacion,
            idSolicitudCredito,
            idSolicitudInfJuridicaInmueble,
            idSolicitudInformacionJuridica,
            numeroMatricula,
        } = rawItem || {};
        return {
            cadastralID: cedulaCatastral,
            cadastralValue: avaluoCatastral,
            commercialValue: avaluoComercial,
            createdDate: fechaCreacion,
            createdFor: creadoPor,
            privateArea: areaPrivada,
            realStateRegistrationNumber: numeroMatricula,
            requestCreditID: idSolicitudCredito,
            requestInformationLegalID: idSolicitudInformacionJuridica,
            requestLegalPropertyID: idSolicitudInfJuridicaInmueble,
        };
    }

    static adaptCreditFlowDocumentList(
        rawList: any[],
    ): CreditFlowDocumentModel[] {
        const flowList: CreditFlowDocumentModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                flowList.push(CreditAdapter.adaptCreditFlowDocument(item));
            });
        }

        return flowList;
    }

    static adaptCreditFlowDocument(rawItem: any): CreditFlowDocumentModel {
        const {
            tcR_ID,
            flU_ID,
            afL_ID,
            dcA_ID,
            tdC_ID,
            tdC_DESCRIPCION,
            dcA_OBLIGATORIO,
            dcA_ORDEN,
            documentO_EXISTE,
        } = rawItem || {};
        return {
            aflID: afL_ID,
            dcaID: dcA_ID,
            dcaOrder: dcA_ORDEN,
            dcaRequired: dcA_OBLIGATORIO,
            fluID: flU_ID,
            tcrID: tcR_ID,
            tdcDescription: tdC_DESCRIPCION,
            tdcID: tdC_ID,
            existDocument: documentO_EXISTE === 'S',
        };
    }

    static adaptCreditSimulation(rawItem: any): CreditSimulationDataModel {
        const {
            numeroDocumento,
            numeroPreaprobado,
            tipoVivienda,
            valorVivienda,
            valorPrestamo,
            tipoAlivio,
            tipoAbono,
            valorTasaEA,
            valorTasaMV,
            valorTasaMVFresh,
            viviendaVIS,
            valorCuota,
            valorCuotaSinSeg,
            plazo,
            fechaSimulacion,
            fechaAprobacion,
            nombreCliente,
            departamento,
            ciudad,
            direccion,
            correoElectronico,
            telefonoFijo,
            telefonoCelular,
            fuerza,
            regimen,
            categoria,
            grado,
            tipoCredito,
            años,
            smcTipoVivienda,
            smcTipoAbono,
            tasaMensual,
            tasaAnual,
        } = rawItem || {};

        return {
            address: direccion,
            approveDate: fechaAprobacion,
            category: categoria,
            cellphone: telefonoCelular,
            city: ciudad,
            clientName: nombreCliente,
            creditType: tipoCredito,
            email: correoElectronico,
            force: fuerza,
            grade: grado,
            identificationNumber: numeroDocumento,
            loanValue: valorPrestamo,
            paymentType: tipoAbono,
            phone: telefonoFijo,
            preApprovedNumber: numeroPreaprobado,
            numerO_PREAPROBADO: numeroPreaprobado,
            propertyType: tipoVivienda,
            propertyValue: valorVivienda,
            quotaValue: valorCuota,
            quotaWithOutSecure: valorCuotaSinSeg,
            rateEAValue: valorTasaEA,
            rateFreshMVValue: valorTasaMVFresh,
            rateMVValue: valorTasaMV,
            regime: regimen,
            reliefType: tipoAlivio,
            simulationDate: fechaSimulacion,
            state: departamento,
            term: plazo,
            visProperty: viviendaVIS,
            years: años,
            tipoVivienda: smcTipoVivienda,
            tipoAbono: smcTipoAbono,
            tasA_MENSUAL: tasaMensual,
            tasA_ANUAL: tasaAnual,
        };
    }

    static adaptConsultCreditData(rawItem: any): CreditConsultCreditModel {
        const {
            homeInsuranceID,
            idSolicitudCredito,
            fechaSolicitud,
            numeroSolicitud,
            idTipoCredito,
            idDepartamento,
            idCiudad,
            slS_ID,
            idAseguradoraHogar,
            idTipoDocumento,
            identificacion,
            declaracionOrigenFondos,
            autorizaUsoDatos,
            autorizaConsultaCentrales,
            declaracionAsegurabilidad,
            convenioAseguradora,
            convenioAseguradoraHogar,
            idAseguradora,
            decisionBuro,
            score,
            capacidadEndeudamiento,
            nivelEndeudamiento,
            nivelEndeudamientoCuota,
            porcentajeExtraprima,
            valorRecomendadoCredito,
            observacionRecomendacion,
            estado,
            creadoPor,
            fechaCreacion,
            actualizadoPor,
            fechaActualiza,
            convenioVida,
            convenioHogar,
            aseguradora,
            tipoAlivio,
            edad,
            valorPrestamo,
        } = rawItem || {};

        return {
            homeInsuranceID: idAseguradoraHogar,
            allowConsultRiskCentrals: autorizaConsultaCentrales,
            allowDataUsage: autorizaUsoDatos,
            bureauDecision: decisionBuro,
            cityID: idCiudad,
            createdDate: fechaCreacion,
            createdFor: creadoPor,
            creditRecommendedValue: valorRecomendadoCredito,
            creditTypeID: idTipoCredito,
            dealHome: convenioHogar,
            dealInsurance: convenioAseguradora,
            dealInsuranceHome: convenioAseguradoraHogar,
            debtCapacity: capacidadEndeudamiento,
            debtLevel: nivelEndeudamiento,
            debtLevelQuota: nivelEndeudamientoCuota,
            declarationInsurability: declaracionAsegurabilidad,
            declarationOriginFunds: declaracionOrigenFondos,
            identification: identificacion,
            insurance: aseguradora,
            insuranceID: idAseguradora,
            lifeAgreement: convenioVida,
            percentageExtraBonus: porcentajeExtraprima,
            recommendedObservation: observacionRecomendacion,
            requestCreditID: idSolicitudCredito,
            requestDate: fechaSolicitud,
            requestNumber: numeroSolicitud,
            score,
            slsID: slS_ID,
            state: estado,
            stateID: idDepartamento,
            typeDocumentID: idTipoDocumento,
            updatedDate: fechaActualiza,
            updatedFor: actualizadoPor,
            reliefType: tipoAlivio,
            edad:edad,
            valorPrestamo:valorPrestamo,
        };
    }

    static adaptCreateSimulationListData(
        rawList: any[],
    ): CreditCreateSimulationDataModel[] {
        const creditSimulationList: CreditCreateSimulationDataModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                creditSimulationList.push(
                    CreditAdapter.adaptCreateSimulationData(item),
                );
            });
        }

        return creditSimulationList;
    }

    static adaptCreateSimulationData(
        rawItem: any,
    ): CreditCreateSimulationDataModel {
        const {
            smD_ID,
            smC_ID,
            cuota,
            fechaCorte,
            fechaPago,
            saldo,
            intereses,
            capital,
            seguroVida,
            seguroHogar,
            totalCuota,
            abonoExtra,
            abonoMensual,
            cuotaUnidadPagadora,
            interesesReduccion,
            cuotaReduccion,
            cobroFonvivienda,
            smlv,
            inflacion,
            incrementoAnual,
            financiaVIS,
            salarioMinVIS,
            financiaNoVIS,
            porcSeguroTerremoto,
            diasDesembolso,
            mesesLeyFrech,
            mesesLeyAliv,
            mesSubsidioSol,
            mesSubsidioOtro,
            tasaEA,
            diasValidoPreaprobado,
            minimoMesesPlazo,
            maximoMesesPlazo,
            estado,
            creadoPor,
            fechaCreacion,
            modificadoPor,
            fechaModificacion,
            canonIniLSG,
            canonFinLSG,
            opcionCompraLSG,
            factorAhorro,
            factorCesantias,
            valorPrestamo,
            valorTasa,
            viviendaVis,
            derechoCuota,
            derechoTasa,
            idSimulacion,
            minimoMeses,
            maximoMeses,
            valorTasaNominal,
            valorTasaNominalF,
            abonoUnico,
            valorAbonoExtra,
            totalIntereses,
            capitalReducido,
            capitalPorPagar,
            interesesPorPagar,
            afectacionCuentaIndividual,
            saldoCapitalReducido,
            valorDeudaRemanente,
            saldoLeasing,
            capitalLeasing,
            capitalpagar,
            capitalSaldo,
            capitalInteresporPagar,
            capitalSaldoTotal,
            interesLeasing,
            interesPorPagar,
            seguroVidaLeasing,
            saldoCapital,
            saldoTotal,
        } = rawItem || {};
        return {
            allowanceMonthOther: mesSubsidioOtro,
            allowanceMonthSol: mesSubsidioSol,
            annualIncrement: incrementoAnual,
            balance: saldo,
            canonFinLSG,
            canonIniLSG,
            capital: capital,
            createdDate: fechaCreacion,
            createdFor: creadoPor,
            cutDate: fechaCorte,
            extraPayment: abonoExtra,
            feeTotal: totalCuota,
            fonViviendaPayment: cobroFonvivienda,
            inflation: inflacion,
            interests: intereses,
            lifeInsurance: seguroVida,
            loanValue: valorPrestamo,
            maxMonthTerms: maximoMesesPlazo,
            maxMonths: maximoMeses,
            minMonthTerms: minimoMesesPlazo,
            minMonths: minimoMeses,
            minSalaryVIS: salarioMinVIS,
            modifiedDate: fechaModificacion,
            modifiedFor: modificadoPor,
            monthLawAlive: mesesLeyAliv,
            monthlyPayment: abonoMensual,
            monthsLawFrech: mesesLeyFrech,
            noVISFinancial: financiaNoVIS,
            nominalRateFValue: valorTasaNominalF,
            nominalRateValue: valorTasaNominal,
            outlayDays: diasDesembolso,
            payerUnitQuota: cuotaUnidadPagadora,
            paymentDate: fechaPago,
            preApprovedValidDays: diasValidoPreaprobado,
            purchaseOptionLSG: opcionCompraLSG,
            quakeInsurance: seguroHogar,
            quakeInsurancePercentage: porcSeguroTerremoto,
            quota: cuota,
            rateEA: tasaEA,
            rateValue: valorTasa,
            reductionInterest: interesesReduccion,
            reductionQuota: cuotaReduccion,
            rightFee: derechoCuota,
            rightRate: derechoTasa,
            savingFactor: factorAhorro,
            severanceFactor: factorCesantias,
            simulationID: idSimulacion,
            smcID: smC_ID,
            smdID: smD_ID,
            smlv,
            state: estado,
            visFinancial: financiaVIS,
            visLiving: viviendaVis,
            abonoUnico: abonoUnico,
            valorAbonoExtra: valorAbonoExtra,
            totalIntereses: totalIntereses,
            capitalReducido:capitalReducido,
            capitalPorPagar: capitalPorPagar,
            interesesPorPagar: interesesPorPagar,
            afectacionCuentaIndividual: afectacionCuentaIndividual,
            saldoCapitalReducido: saldoCapitalReducido,
            valorDeudaRemanente: valorDeudaRemanente,
            saldoLeasing: saldoLeasing,
            capitalLeasing: capitalLeasing,
            capitalpagar: capitalpagar,
            capitalSaldo: capitalSaldo,
            capitalInteresporPagar: capitalInteresporPagar,
            capitalSaldoTotal: capitalSaldoTotal,
            interesLeasing: interesLeasing,
            interesPorPagar: interesPorPagar,
            seguroVidaLeasing: seguroVidaLeasing,
            saldoCapital: saldoCapital,
            saldoTotal: saldoTotal
        };
    }
}

export default CreditAdapter;
