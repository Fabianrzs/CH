import axios from "axios";
import { toast } from "react-toastify";
import StorageService from "./storage.service";
import { routega2api } from '../utils/baserouteapi.route';
import { CuentaDepositoDto, DespachosJudiciales, JuzgadoDto } from "../types/court.model.type";
import { juzgadoForm } from "../pages/court/constants/courtform.model.type";


const ApiEmbargo = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const postAllCourtsByDepartament = (departament: number) =>
        apiAxios
            .post(`Embargos/ObtenerJuzgadosPorDepartamento?idDpto=${departament}`,
                { idDpto: departament }, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const AgregarTipoActorEmbargo = (tipoActor: any) =>
        apiAxios
            .post(`Embargos/TipoActor`,
                tipoActor, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const EliminarTipoActorEmbargo = (tipoActor: any) =>
        apiAxios
            .delete(`catalogos/EliminarTipoActorEmbargo?request=${tipoActor}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const postAllCourtsByCity = (departament: number, city: number) =>
        apiAxios
            .post(`Embargos/ObtenerJuzgadosPorCiudad`,
                {
                    idDepartamento: departament,
                    idCiudad: city
                }, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertCourts = (juzgado: JuzgadoDto) =>
        apiAxios
            .post(`Embargos/CrearJuzgado`,
                juzgado, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postUpdateCourts = (juzgado: JuzgadoDto) =>
        apiAxios
            .post(`Embargos/ActualizarJuzgado`,
                juzgado, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const deleteCourts = (juzgado: JuzgadoDto) => {

        apiAxios
            .delete(`Embargos/EliminarJuzgados?request=${juzgado.idJuzgado}`, {
                baseURL: routega2api,
            })
            .then(async res => res.data);

    }

    const getCourtById = (idJuzgado: string) =>
        apiAxios
            .get(`ObtenerJuzgadoById?request=${idJuzgado}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAllCourtsByCityCode = (despachosJudiciales: DespachosJudiciales) =>
        apiAxios
            .post(`Embargos/ObtenerJuzgadosPorDepartamentoCiudadCodigo`,
                despachosJudiciales, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const postAllCourtsByCodeAndAccountnumber = (cuentaDeposito: CuentaDepositoDto) =>
        apiAxios
            .post(`Embargos/ObtenerCuentaCodigo`,
                cuentaDeposito,
                {
                    baseURL: routega2api,
                })
            .then(async res => res.data);


    return {
        postAllCourtsByDepartament,
        postAllCourtsByCity,
        postInsertCourts,
        postUpdateCourts,
        deleteCourts,
        getCourtById,
        postAllCourtsByCityCode,
        postAllCourtsByCodeAndAccountnumber,
        AgregarTipoActorEmbargo,
        EliminarTipoActorEmbargo
    };

};

export default ApiEmbargo;