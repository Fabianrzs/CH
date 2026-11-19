import React, { createContext, useContext, useEffect, useState } from "react";
import CatalogApi from "../../services/api-catalog.service";

const IpContext = createContext<any>(null);

export const IpProvider = ({ children }: { children: React.ReactNode }) => {
  const [publicIp, setPublicIp] = useState<string | null>(null);
  const [localIp, setLocalIp] = useState<string | null>(null);
  const catalogApi = CatalogApi();

  useEffect(() => {
    const fetchPublicIPAddress = async () => {
      try {
        const response = await fetch("https://api.ipify.org?format=json");
        const data = await response.json();
        setPublicIp(data.ip);
      } catch (error) {
        console.error("Error obteniendo IP pública:", error);
      }
    };

    const fetchLocalIPAddress = () => {
      const peerConnection = new RTCPeerConnection({ iceServers: [] });
      peerConnection.createDataChannel("");

      peerConnection.createOffer()
        .then((offer) => peerConnection.setLocalDescription(offer))
        .catch((error) => console.error("Error creando oferta:", error));

      peerConnection.onicecandidate = (event) => {
        if (event.candidate) {
          const ip: any = event.candidate.address;
          setLocalIp(ip);
          enviarIp(ip);
        }
      };
    };

    const enviarIp = async (ip: any) => {
      try {
        const response = await catalogApi.getObtenerIpLocal(ip);
        if (response?.isSuccess) {
          console.log("IP obtenida del API:", response.data);
          setPublicIp(response.data);
        }
      } catch (error) {
        console.error("Error al obtener la IP local:", error);
      }
    };

    fetchPublicIPAddress();
    fetchLocalIPAddress();
  }, []);

  return (
    <IpContext.Provider value={{ publicIp, localIp }}>
      {children}
    </IpContext.Provider>
  );
};

export const useIp = () => useContext(IpContext);
