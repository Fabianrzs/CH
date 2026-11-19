import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import { toast } from "react-toastify";
import { startGetClientByIdentificationTypeIdentificationNumber, startGetMovementsAccountsClient } from "../../actions/client.action";
import { startObtenerVendedorCredito } from "../../actions/credit.actions";
import CreditApi from "../../services/api-credit.service";
import { AppState } from "../../store/rootReducer";
import UtilService from "../../utils/util.service";
import CHPaper from "../ch-paper/CHPaper";
import InformacionBeneficiario from "../InformacionBeneficiario/InformacionBeneficiario";

const SolicitudesNotificaciones = (
    { procedureRequest,
        isLoading,
        CreditState,
    }: { procedureRequest: any; isLoading: boolean; CreditState: any; }) => {

    const creditApi = CreditApi();
    const [listaVendedor, setListaVendedor] = useState([])
    const [cdpDatos, setCdpDatos] = useState<any>({})
    const [usariosAprobados, setUsariosAprobados] = useState<any>([])
    const [valorNuevo1, setValorNuevo1] = useState(0);
    const [datosDesembolso, setDatosDesembolso] = useState<any>([]);
    const [valorNuevo2, setValorNuevo2] = useState(0);
    const [datosCuotaInicial, setDatosCuotaInicial] = useState<any>([]);
    const [valorDesembolso, setValorDesembolso] = useState<any>(0);

    useEffect(() => {
        if (procedureRequest.activity === 'PRIMER PAGO') {
            mostrarFuentes();
        }
    }, [])

    useEffect(() => {
        const request = {
            Soc_Id: procedureRequest.requestID,
            Sls_Id: '00000000-0000-0000-0000-000000000000'
        }
        creditApi
            .postObtenerVendedor(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {
                        setListaVendedor(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }, [])
    console.log({ procedureRequest })
    useEffect(() => {
        
        const request = procedureRequest.simulationID
        creditApi
            .postConsultarNotificacionesSolicitudesCredito(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {
                        setCdpDatos(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])
    useEffect(() => {
        const request = procedureRequest.trsID
        creditApi
            .PostConsultarAprobacionAsesorCredito(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {
                        setUsariosAprobados(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    const mostrarFuentes = async () => {
        const dataRequest = procedureRequest.requestID;
        await creditApi
            .getFuentes(dataRequest)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        const ordenarRecursosCaja = [...response.data.filter((x: any) =>
                            x.idFuenteRecursos === 3)]
                        const index = ordenarRecursosCaja.length;
                        let sumaDesembolso = 0
                        const ordenarRecursosAfiliadoCuentas = [...response.data.filter((x: any) =>
                            x.idFuenteRecursos === 2)]
                        const index1 = ordenarRecursosAfiliadoCuentas.length;
                        let sumaordenarRecursosAfiliadoCuentas = 0
                        const ordenarRecursosAfiliado = [...response.data.filter((x: any) =>
                            x.idFuenteRecursos === 1), ...response.data.filter((x: any) =>
                                x.idFuenteRecursos === 2)]
                        const index2 = ordenarRecursosAfiliado.length;
                        let sumaRecursosCuotaInicial = 0
                        setValorDesembolso(response?.data[0]?.valorDesembolso)
                        if (index > 0) {
                            for (let i = 0; i < index; i += 1) {
                                const element = ordenarRecursosCaja[i]
                                sumaDesembolso += element.valorDesembolso
                            }
                            setValorNuevo1(sumaDesembolso)
                            const newData = [...ordenarRecursosCaja];
                            const newRow = {
                                descripcionFuenteRecursos: "TOTAL",
                                valorDesembolso: sumaDesembolso,
                                fechaDesembolso: "",
                            };
                            newData.push(newRow);
                            setDatosDesembolso(newData)
                        } else {
                            setDatosDesembolso([])
                        }
                        if (index1 > 0 || index2 > 0) {
                            for (let i = 0; i < index1; i += 1) {
                                const element = ordenarRecursosAfiliadoCuentas[i]
                                sumaordenarRecursosAfiliadoCuentas += element.valorDesembolso
                            }
                            setValorNuevo2(sumaordenarRecursosAfiliadoCuentas)
                            for (let i = 0; i < index2; i += 1) {
                                const element = ordenarRecursosAfiliado[i]
                                sumaRecursosCuotaInicial += element.valorDesembolso
                            }
                            const newData = [...ordenarRecursosAfiliado];
                            const newRow = {
                                descripcionFuenteRecursos: "TOTAL",
                                valorDesembolso: sumaRecursosCuotaInicial,
                                fechaDesembolso: "",
                            };
                            newData.push(newRow);
                            setDatosCuotaInicial(newData)

                        } else {
                            setDatosCuotaInicial([])
                        }
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            })
    };

    const getMessage = () => {
        
        switch (procedureRequest.activity) {

            case 'APROBACIÓN 1':

                return (
                    <div>
                        <Typography style={{ color: 'black' }}>
                            Se requiere de su aprobación para el paso a la siguiente instancia.
                        </Typography>
                    </div>
                );
            case 'APROBACIÓN 2':
                const nombreUsuarioDos = usariosAprobados?.filter((usuario: any) => usuario.taC_DESCRIPCION === "APROBACIÓN 1")
                return (
                    <Typography style={{ color: 'black' }}>
                        Cuenta con la aprobación de <strong> {nombreUsuarioDos[0]?.nombres} </strong>  y se requiere de su aprobación para el paso a la siguiente instancia.
                    </Typography>
                );
            case 'APROBACIÓN 3':
                const nombreUsuarioTres = usariosAprobados?.filter((usuario: any) => usuario.taC_DESCRIPCION === "APROBACIÓN 2" || usuario.taC_DESCRIPCION === "APROBACIÓN 1")
                return (
                    <Typography style={{ color: 'black' }}>
                        Cuenta con la aprobación de <strong> {nombreUsuarioTres[0]?.nombres} </strong>  y <strong> {nombreUsuarioTres[1]?.nombres} </strong>   y se requiere de su aprobación para el paso a la siguiente instancia.
                    </Typography>
                );
            case 'FINANCIERA':
                const nombreUsuarioFinanciero = usariosAprobados?.filter((usuario: any) => usuario.taC_DESCRIPCION === "APROBACIÓN 1" || usuario.taC_DESCRIPCION === "APROBACIÓN 2" || usuario.taC_DESCRIPCION === "APROBACIÓN 3")
                return (
                    <Typography style={{ color: 'black' }}>
                        Cuenta con las aprobaciones de <strong> {nombreUsuarioFinanciero[0]?.nombres} </strong> , de <strong> {nombreUsuarioFinanciero[1]?.nombres} </strong>  y de <strong> {nombreUsuarioFinanciero[2]?.nombres} </strong> . Si considera que hay alguna anomalía en esta solicitud, por favor devuélvala a la instancia anterior, de lo contrario, por favor apruébela.
                    </Typography>
                );
            case 'CONTABILIDAD':
                const nombreUsuarioContabilidad = usariosAprobados?.filter((usuario: any) => usuario.taC_DESCRIPCION === "APROBACIÓN 1" || usuario.taC_DESCRIPCION === "APROBACIÓN 2" || usuario.taC_DESCRIPCION === "APROBACIÓN 3")
                return (
                    <Typography style={{ color: 'black' }}>
                        Cuenta con las aprobaciones de <strong> {nombreUsuarioContabilidad[0]?.nombres} </strong> , de <strong>  {nombreUsuarioContabilidad[1]?.nombres} </strong> y de <strong> {nombreUsuarioContabilidad[2]?.nombres} </strong> . Si considera que hay alguna anomalía en esta solicitud, por favor devuélvala a la instancia anterior, de lo contrario, por favor apruébela.
                    </Typography>
                );
            case 'DESEMBOLSO':
                const nombreUsuarioDesembolso = usariosAprobados?.filter((usuario: any) => usuario.taC_DESCRIPCION === "APROBACIÓN 1" || usuario.taC_DESCRIPCION === "APROBACIÓN 2" || usuario.taC_DESCRIPCION === "APROBACIÓN 3")
                return (
                    <Typography style={{ color: 'black' }}>
                        Cuenta con las aprobaciones de <strong> {nombreUsuarioDesembolso[0]?.nombres} </strong> , de <strong> {nombreUsuarioDesembolso[1]?.nombres} </strong>  y de <strong> {nombreUsuarioDesembolso[2]?.nombres} </strong> . Si considera que hay alguna anomalía en esta solicitud, por favor devuélvala a la instancia anterior, de lo contrario, por favor apruébela.
                    </Typography>
                );
            default:
                return null;
        }
    };
    const segPago = 'DESEMBOLSO SEGUNDO PAGO'

    return (
        <>
            <br />
            {
                procedureRequest.activity === 'PRIMER PAGO' ?
                    <div>
                        <CHPaper title="Listado Recursos - Primer Pago">
                            <Typography> Ha sido aprobado realizar el primer pago por valor total de <strong>{UtilService.formatCurrencyTres(valorDesembolso)}</strong> , provenientes de la cuenta individual del afiliado <strong> {procedureRequest?.clientName}</strong>, con número de identificación <strong>{procedureRequest?.clientIdentificationNumber}</strong>, de la siguiente manera:</Typography>
                            <br />
                            <Table>
                                <TableHead>
                                    <TableCell style={{ width: "25%" }}>Tipo Fuente</TableCell>
                                    <TableCell style={{ width: "25%" }}>Beneficiario Pago</TableCell>
                                    <TableCell style={{ width: "20%" }}>Valor</TableCell>
                                    <TableCell style={{ width: "20%" }}>Fecha</TableCell>
                                </TableHead>
                                <TableBody>
                                    {datosCuotaInicial.map((dato: any) => (

                                        <TableRow key={`row-${dato.idSolicitudDesembolso}`}>
                                            <TableCell>
                                                {dato.descripcionFuenteRecursos}
                                            </TableCell>
                                            <TableCell>
                                                {(CreditState.formularioVendedor.find((item: any) => item.id === dato.vendedor_Id))?.nombre_Vendedor}
                                            </TableCell>
                                            <TableCell>
                                                {UtilService.formatCurrency(
                                                    dato.valorDesembolso,
                                                )}
                                            </TableCell>
                                            <TableCell>
                                                {UtilService.getDate(dato.fechaDesembolso)}
                                            </TableCell>

                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </CHPaper>
                        <InformacionBeneficiario procedureRequest={procedureRequest} />
                    </div> :
                    <CHPaper title={procedureRequest.activity === 'DESEMBOLSO' ? segPago : procedureRequest.activity}>
                        <div>
                            <Typography style={{ color: 'black' }}>
                                En la fecha {UtilService.getDate(cdpDatos?.noT_FECHA)} se generó la solicitud de un  <strong> CDP {cdpDatos?.noT_CDP}</strong> por valor de <strong>{UtilService.formatCurrency(procedureRequest.creditValue)}</strong>, por petición de <strong>ANA VARGAS</strong> , en el proceso de legalización, con la siguiente información:
                            </Typography>
                        </div>
                        <br />
                        <br />
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow style={{ backgroundColor: '#1C3F8B' }}>
                                        <TableCell style={{ color: 'white' }}><strong>Nombres</strong> </TableCell>
                                        <TableCell style={{ color: 'white' }}> <strong>Tipo Tercero</strong> </TableCell>
                                        <TableCell style={{ color: 'white' }}> <strong>No. Identificación</strong> </TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow >
                                        <TableCell>{procedureRequest?.clientName}</TableCell>
                                        <TableCell>Afiliado</TableCell>
                                        <TableCell>{procedureRequest?.clientIdentificationNumber}</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </TableContainer>
                        <br />
                        <br />
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow style={{ backgroundColor: '#1C3F8B' }}>
                                        <TableCell style={{ color: 'white' }}><strong>Nombres</strong> </TableCell>
                                        <TableCell style={{ color: 'white' }}> <strong>Tipo Tercero</strong> </TableCell>
                                        <TableCell style={{ color: 'white' }}> <strong>No. Identificación</strong> </TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {listaVendedor.map((row: any) => (
                                        <TableRow key={row.id}>
                                            <TableCell>{row.nombre_Vendedor}</TableCell>
                                            <TableCell>Beneficiario</TableCell>
                                            <TableCell>{row.numero_Doc}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                        <br />
                        <div>
                            {getMessage()}
                        </div>
                    </CHPaper>
            }
        </>
    )
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    CreditState: state.credit,

});
const mapDispatchToProps = {
    startGetMovementsAccountsClient,
    startGetClientByIdentificationTypeIdentificationNumber,
    startObtenerVendedorCredito,
};

export default connect(mapStateToProps, mapDispatchToProps)(SolicitudesNotificaciones)