import { isArray } from 'lodash';
import { ActivityStatusModel } from '../models/activity-status.model';
import { CreditTypeModel } from '../models/credit-type.model';

class CatalogAdapter {
    static adaptActivityStatusList(rawList: any[]): ActivityStatusModel[] {
        const activitiesStatus: ActivityStatusModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                activitiesStatus.push(CatalogAdapter.adaptActivityStatus(item));
            });
        }

        return activitiesStatus;
    }

    static adaptActivityStatus(rawItem: any): ActivityStatusModel {
        const {
            id,
            descripcion,
            creadoPor,
            esA_FECHA_CREACION,
            esA_MODIFICADO_POR,
            esA_FECHA_MODIFICACION,
        } = rawItem || {};

        return {
            id: id,
            createdFor: creadoPor,
            creationDate: esA_FECHA_CREACION,
            description: descripcion,
            modifiedDate: esA_FECHA_MODIFICACION,
            modifiedFor: esA_MODIFICADO_POR,
        };
    }

    static adaptCreditTypeList(rawList: any[]): CreditTypeModel[] {
        const creditTypeList: CreditTypeModel[] = [];

        if (isArray(rawList)) {
            rawList.forEach(item => {
                creditTypeList.push(CatalogAdapter.adaptCreditType(item));
            });
        }

        return creditTypeList;
    }

    static adaptCreditType(rawItem: any): CreditTypeModel {
        const {
            tcR_ID,
            tcR_DESCRIPCION,
            tcR_ID_PADRE,
            tcR_NIVEL,
            tcR_ESTADO,
            tcR_CREADO_POR,
            tcR_FECHA_CREACION,
            tcR_MODIFICADO_POR,
            tcR_FECHA_MODIFICACION,
        } = rawItem || {};

        return {
            id: tcR_ID,
            description: tcR_DESCRIPCION,
            parentID: tcR_ID_PADRE,
            level: tcR_NIVEL,
            state: tcR_ESTADO,
            createdFor: tcR_CREADO_POR,
            creationDate: tcR_FECHA_CREACION,
            modifiedFor: tcR_MODIFICADO_POR,
            modifiedDate: tcR_FECHA_MODIFICACION,
        };
    }
}

export default CatalogAdapter;
