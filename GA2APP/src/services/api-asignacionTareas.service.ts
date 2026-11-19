import axios, { AxiosResponse } from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';


const asigTareas = () => {
	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/json',
			'accept': 'application/json',
			'X-Frame-Options': 'sameorigin'
		},

		baseURL: `${routega2api}`,
		timeout: 120000,
	});


	apiAxios.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});


	const postAsignarTarea = async (data: any) =>
		apiAxios
			.post('AsignacionDeTareas', data)
			.then((res: any) => res.data);
		
    const getObtenerAsignacionTareas = (request: any) =>
        apiAxios
            .get(
                `ConsultarAsignacionTareas?idUsuario=${request.idUsuario}`,
                {
                    baseURL: routega2api,
                },
            )
            .then((res: any) => res.data);

			const getConsultaNotificacionesCargueNomina =  (idUsuario: any) =>
			  apiAxios
				.get(`ConsultaNotificacionesCargueNomina?request=${idUsuario}`,
				{
					baseURL: routega2api,
				})
				.then((res: any) => res.data);
		  

	return {
		postAsignarTarea,
        getObtenerAsignacionTareas,
		getConsultaNotificacionesCargueNomina,
		
	};
};

export default asigTareas;
