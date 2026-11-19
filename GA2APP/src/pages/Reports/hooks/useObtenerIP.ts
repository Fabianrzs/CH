import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import CatalogApi from '../../../services/api-catalog.service';

const useObtenerIP = () => {
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) =>
        /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(ip);

    const isValidIPv6 = (ip: string) =>
        /([a-f0-9:]+:+)+[a-f0-9]+/i.test(ip);

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0];
        await catalogApi
            .getObtenerIpLocal(convertIp)
            .then((response: any) => {
                if (response?.isSuccess) {
                    setPublicIpAddress(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    useEffect(() => {
        const peerConnection = new RTCPeerConnection({ iceServers: [] });
        peerConnection.createDataChannel('');
        peerConnection.createOffer()
            .then((offer) => peerConnection.setLocalDescription(offer))
            .catch((error) => console.error('Error creating offer:', error));

        peerConnection.onicecandidate = (event) => {
            if (event.candidate) {
                const ip: any = event.candidate.address;
                if (isValidIPv4(ip) || isValidIPv6(ip)) {
                    enviarIp(ip);
                } else {
                    enviarIp(ip);
                }
            }
        };

        return () => {
            peerConnection.close();
        };
    }, []);

    return publicIpAddress;
};

export default useObtenerIP;
