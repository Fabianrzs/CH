import axios, { AxiosResponse } from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';


const afiliadoApi = () => {
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


	
	const postGuardarValidacionTelefonica = async (data: any) =>
		apiAxios
			.post('VerificacionTelefonoAfl/CrearVerificacionTelefonoAfl', data)
			.then((res: any) => res.data);
			
	const postObtenerValidacionTelefonica = async (data: any) =>
		apiAxios
			.post(`VerificacionTelefonoAfl/ObtenerVerificacionTelefonoAfl?Soc_Id=${data.soc_Id}`)
			.then((res: any) => res.data);

	const postGuardarBeneficiario = async (data: any) =>
		apiAxios
			.post('InfBeneficiario/CreaInfoBeneficiario', data)
			.then((res: any) => res.data);	

	const postCrearBeneficiario = async (data: any) =>
		apiAxios
			.post(`InfBeneficiario/CrearInformacionBeneficiario`, data)
			// .post('InfBeneficiario/CreaInfoBeneficiario', data)
			.then((res: any) => res.data);	

	const postObtenerBeneficiario = async (data: any) =>
		apiAxios
				// .post(`InfBeneficiario/CrearInformacionBeneficiario?Soc_Id=${data.soc_Id}`)
				.post(`InfBeneficiario/ObtenerInformacionBeneficiario?Soc_Id=${data.soc_Id}`)
				.then((res: any) => res.data);


	const postAliadosEstrategicos = async (data: any) =>
		apiAxios
			.post('CrearAliadoEstrategico', data)
			.then((res: any) => res.data);

	const postObtenerAliadosEstrategicos = async (data: any) =>
		apiAxios
			.post(`ObtenerDatosAliadoEstrategico?Soc_id=${data.soc_Id}`)
			.then((res: any) => res.data);


	const postActulaizarAliadoEstrategico = async (data: any) =>
		apiAxios
			.post('ActualizarAliadoEstrategico', data)
			.then((res: any) => res.data);


	const EliminarInfBeneficiario = (index: any) =>
		apiAxios
			.delete(`InfBeneficiario/EliminarInformacionBeneficiario?Id=${index.Id}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);	
			

	return {
		postGuardarValidacionTelefonica,
		postObtenerValidacionTelefonica,
		postGuardarBeneficiario,
		postObtenerBeneficiario,
		postAliadosEstrategicos,
		postObtenerAliadosEstrategicos,
		postActulaizarAliadoEstrategico,
		postCrearBeneficiario,
		EliminarInfBeneficiario,
		
	};
};

export default afiliadoApi;
