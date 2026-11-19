import axios from "axios";
import { Auditoria, ConsultaAuditoria } from "../types/Auditoria.model.type";
import { routega2api } from "../utils/baserouteapi.route";

const ApiAuditorias = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 15000,
    });

    const GetTablasAuditoria = () =>
        apiAxios
            .get('ObtenerTablasAuditoria', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetLogAuditoria = async (auditoria: ConsultaAuditoria) => 
        apiAxios
            .post(`ObtenerlogAuditoria`, auditoria, {
                baseURL: routega2api 
            })
            .then(res => res.data);

    const InsertAuditoria = async (auditoria: Auditoria) => 
    
        apiAxios
            .post(`InsertarLogAuditoria`, auditoria, {
                baseURL: routega2api 
            })
            .then(res => res.data);

       

    return {

        GetTablasAuditoria,
        GetLogAuditoria,
        InsertAuditoria
    };

}

export default ApiAuditorias;