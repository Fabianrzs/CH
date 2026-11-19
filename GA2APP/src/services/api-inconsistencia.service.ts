import axios from 'axios';
import { routega2api } from '../utils/baserouteapi.route';



const Inconsistencia = () => {

	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/json',
			'X-Frame-Options': 'sameorigin'
		},
		baseURL: `${routega2api}`,
		timeout: 60000,
	});


		

            const GetInconsistencias = (data: string) =>
            apiAxios
            .get(`SP_CONSULTAR_INCONSISTENCIAS_SOLICITUD?IdSolicitud=${data}`)
                .then((res: any) => res.data);
    

	return {
	
		GetInconsistencias
	};
};

export default Inconsistencia;
