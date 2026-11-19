import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Prospeccion from "../../services/api-prospeccion.service";
import UtilService from "../../utils/util.service";
import CHPaper from "../ch-paper/CHPaper";
import CHPaperV2 from "../ch-paper/CHPaperV2";
import { LoadingComponent } from "../loading/loading.component";
import SegundaTablaRecursosACongelar from "./SegundaTablaRecursosACongelar";


const RecursosACongelar = ({procedureRequest, isLoading}: {procedureRequest:any, isLoading:any} ) => {

    const [listadoRecursosCongelados, setlistadoRecursosCongelados] = useState<any[]>([])
    const [listadoRecursosCongeladosTablaDos, setlistadoRecursosCongeladosTablaDos] = useState<any[]>([])
    const [datosUsuario, setDatosUsuario] = useState<any>({})
    const [loading, setLoading] = useState(false)


    useEffect(()=>{
        obtenerRecursosCongelados()   
        obtenerRecursosCongeladosTablaDos()   
    },[])

    const obtenerRecursosCongelados = () => {
        setLoading(true)
      
        const apiProspeccion = Prospeccion()
        const request = {
            slS_ID: procedureRequest.simulationID,
            coN_NUMERO_DOCUMENTO: String(procedureRequest.clientIdentificationNumber),
            coN_TIPO_IDENTIFICACION: 1
            //         slS_ID: "8433233C-715D-492C-9FE5-AA15CD64D2B0",
            //   coN_NUMERO_DOCUMENTO: "1024519115",
            //         coN_TIPO_IDENTIFICACION:1      

        }
        apiProspeccion
            .postObtenerRecursosConelados(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {

                        
                      setlistadoRecursosCongelados(response?.data?.listaRecursosCongelados)
                      setDatosUsuario(response?.data)
                      setLoading(false)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setLoading(false)
            });
    }

    const ahorros = listadoRecursosCongelados.filter((item: any) =>
    ["AHORROS OBLIGATORIOS", "AHORROS RETROACTIVOS", "AHORROS VOLUNTARIOS", "INTERESES APORTES"].includes(item.coN_DESCRIPCION)
);

const cesantias = listadoRecursosCongelados.filter((item: any) =>
    ["CESANTÍAS", "CESANTÍAS RETROACTIVOS", "INTERESES CESANTÍAS"].includes(item.coN_DESCRIPCION)
);
    const obtenerRecursosCongeladosTablaDos = () => {
        setLoading(true)
        console.log({procedureRequest})
        const apiProspeccion = Prospeccion()
        const request = {
            slS_ID: procedureRequest.simulationID,
            coN_NUMERO_DOCUMENTO: String(procedureRequest.clientIdentificationNumber),
            coN_TIPO_IDENTIFICACION: 1,
            coN_TIPO_CREDITO: procedureRequest.creditType,
        }
        apiProspeccion
            .postConsultarConceptosCongeladosCalculados(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {
                   
                   setlistadoRecursosCongeladosTablaDos(response?.data?.listaRecursosCongelar)
                    //   setDatosUsuario(response?.data)
                     
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
              
            });
    }
//     const getMessage = () => {
//       switch (listadoRecursosCongelados[0]?.cgR_TIPO_CREDITO) {
//           case "LEASING 108-168":
//               return (
//                   <div>
//                       <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           case 'AFILIADO VETERANO':
//               return (
//                   <div>
//                       <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );


//           case 'AFILIADO SOLUCION VIVIENDA':
//               return (
//                   <div>
//                    <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           case 'LEASING 24-107':
//               return (
//                   <div>
//                       <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           case 'LEASING CESANTÍAS':
//               return (
//                   <div>
//                      <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           case 'FUNCIONARIO AFILIADO SOLUCIÓN VIVIENDA':
//               return (
//                   <div>
//                      <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           case 'FUNCIONARIO AFILIADO ADMINISTRACIÓN CESANTÍAS':
//               return (
//                   <div>
//                       <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
          
//           case 'AFILIADO ADMINISTRACION CESANTIAS':
//               return (
//                   <div>
//                   <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           case 'BENEFICIARIO':
//               return (
//                   <div>
//                      <Typography style={{ color: 'black' }}>
//                       La siguiente es al relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
//                       </Typography>
//                   </div>
//               );
//           default:
//               return null;
//       }
//   };

    return (
      <>
       <LoadingComponent key="loader" loading={loading } />
          
    <br />
      
      <CHPaper title="Cuenta individual al prospectar">

      <Typography style={{ color: 'black' }}>
                La siguiente es la relación de los recursos de la cuenta individual correspondientes al afiliado <strong>{datosUsuario.primerNombre} {datosUsuario?.segundoNombre} {datosUsuario?.primerApellido}  {datosUsuario?.segundoApellido}</strong> , con CC <strong>{datosUsuario.numeroDocumento}</strong> , los cuales a la fecha de prospección eran:
            </Typography>
            <br />
            <br />
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow style={{ backgroundColor: '#1C3F8B' }}>
                            <TableCell style={{ color: 'white' }}>Descripción</TableCell>
                            <TableCell style={{ color: 'white' }} align="right">Valor</TableCell>
                            
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {/* Ahorros Subtitle */}
                        <TableRow>
                            <TableCell colSpan={4}>
                                <Typography variant="h6" style={{ fontWeight: 'bold', color: '#1C3F8B' }}>Ahorros</Typography>
                            </TableCell>
                        </TableRow>
                        {ahorros.map((item: any) => (
                            <TableRow key={item.coN_ID}>
                                <TableCell>{item.coN_DESCRIPCION}</TableCell>
                                <TableCell align="right">{UtilService.formatCurrencyTres(item.coN_VALOR)}</TableCell>
                               
                            </TableRow>
                        ))}

                        {/* Cesantías Subtitle */}
                        <TableRow>
                            <TableCell colSpan={4}>
                                <Typography variant="h6" style={{ fontWeight: 'bold', color: '#1C3F8B' }}>Cesantías</Typography>
                            </TableCell>
                        </TableRow>
                        
                        {cesantias.map((item: any) => (
                            <TableRow key={item.coN_ID}>
                                <TableCell>{item.coN_DESCRIPCION}</TableCell>
                                <TableCell align="right">{UtilService.formatCurrencyTres(item.coN_VALOR)}</TableCell>
                           
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        <br />
        <br />
        <br />
    

       
        
        </CHPaper>

        <CHPaper title="Recursos a bloquear">
        <SegundaTablaRecursosACongelar 
        crearMensaje = {listadoRecursosCongelados[0]?.cgR_TIPO_CREDITO}
        listadoRecursosCongeladosTablaDos = {listadoRecursosCongeladosTablaDos}/>
        </CHPaper>
      </>
       
        
      );
}

export default RecursosACongelar