import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';


const CesantiasApi = () => {
	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/json',
			'accept': 'application/json',
			'X-Frame-Options': 'sameorigin'
		},

		baseURL: `${routega2api}cesantias`,
		timeout: 180000,
	});

	const apiAxiosDos = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/json',
			'accept': 'application/json'
		},

		baseURL: `${routega2api}credito`,
		timeout: 120000,
	});


	apiAxios.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	const PostPagosCesantiasSeven = async (data: any) => 
		apiAxios
			.post(`pagosCesantiasSeven`, data)
			.then((res: any) => res.data);


	
	const PostDesEmbolsoCredito = async (data: any) =>
	apiAxiosDos
			.post(`DesEmbolsoCredito`, data)
			.then((res: any) => res.data);

				
	return {
	
        PostPagosCesantiasSeven,
		PostDesEmbolsoCredito
	};
};

export default CesantiasApi;
