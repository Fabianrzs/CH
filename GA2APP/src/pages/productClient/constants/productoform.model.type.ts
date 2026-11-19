import { ProductoDto, 
   AplicacionPago, 
   ErrorListType,
   procesoAplicacionMasivaPago} from '../../../types/producto.model.type';

export const ProductForm: ProductoDto = {
idCredito: '',
identificacion: '',   
edad: 0,
// Categoria
// Cargo/Grado
valorVivienda: 0,
idEstadoVivienda: 0, 
idTipoVivienda: 0,
claseInmueble: '',
noMatricula: '',
escritura: 0,
fechaEscritura: new Date(),
notaria: '',
linderos: '',
direccionInmueble: '',
ciudadInmueble: '',
descripcionInmueble: '',

idTipoCredito: 0,
numeroCredito: 0,
fechaSolicitud: new Date(),
valorDesembolso: 0,
fechaDesembolso: new Date(),
valorCanonInicial: 0,
valorAbonoExtra: 0, // canon extraordinario
canonExtraordinario: 0,
valorCuota: 0,
diaPago: 0,
fechaProximoPago: new Date(),
fechaUltimoPago: new Date (),
estadoCredito: '',  
fechaEstadoCredito: new Date(),
diasMora: 0,
numeroCuotasCanceladas: 0,
cuotasPendientes: 0,
valorOpcionCompra: 0,  
porcentajeOpcionCompra: 0,
valorInteresCorriente: 0,
valorDeudaRemanente: 0, // causacion acumulada interes remanente
valorSeguroVida: 0,
valorSeguroHogar: 0,
  
idEstadoCredito: 0,
tipoAbonoExtra: '',
montoInicial: 0,
saldoCapital: 0,
saldoTotal:0,
saldoCapitalMora: 0,
plazoActual: 0,
plazoTotal: 0,
tasaCredito: 0,
tasaCreditoEA: 0,
tasaCreditoPer: 0,
tasaFrech: 0,
valorAlivioCuota: 0,
alivioFrech: 0,
tasaSeguroHogar: 0,
tasaSeguroVida: 0,
acumuladoInteresCorriente: 0,
ultimoPagoInteresCorriente: new Date (),
acumuladoInteresMora: 0,
ultimoPagoInteresMora: new Date (),
acumuladoSeguroHogar: 0,
acumuladoMoraSeguroHogar: 0,
ultimoPagoSeguroHogar: new Date (),
acumuladoSeguroVida: 0,
acumuladoMoraSeguroVida: 0,
ultimoPagoSeguroVida: new Date (),
score: 0,
fechaFinPagos: new Date ()
};

export const AplicacionPagoForm: AplicacionPago = {
idAplicacionPago: '',
idCredito: '',
fechaPago: new Date (),
fechaAplicacion: new Date(),
abonoSeguroHogar: 0,
abonoSeguroVida: 0,
abonoInteresCorriente: 0,
abonoInteresMora: 0,
abonoCapital: 0,
valorPago: 0,
idDct: 0
}

export const ErroresForm: ErrorListType = {
diaPago: {
  error: false,
  mensaje: ''
},
diasMora: {
  error: false,
  mensaje: ''
},
valorVivienda: {
  error: false,
  mensaje: ''
},
valorDesembolso: {
  error: false,
  mensaje: ''
},
plazoTotal: {
  error: false,
  mensaje: ''
},
saldoCapital: {
  error: false,
  mensaje: ''
},
capitalMora: {
  error: false,
  mensaje: ''
},
tasaSeguroVida: {
  error: false,
  mensaje: ''
},
tasaSeguroHogar: {
  error: false,
  mensaje: ''
},
alivioFrech: {
  error: false,
  mensaje: ''
},
tasaCredito: {
  error: false,
  mensaje: ''
},
tasaFrech: {
  error: false,
  mensaje: ''
}
}

export const procesoAplicacionMasivaPagoForm: procesoAplicacionMasivaPago = {
fechaAplicacion: null,    
valor: null,
archivo: null,
aplicanPagos: null,
formatoAplicar: null,
creadoPor: null
}