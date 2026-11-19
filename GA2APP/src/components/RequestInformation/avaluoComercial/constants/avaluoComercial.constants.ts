import { FormValues } from "../types/avaluoComercial.types";

export const initialValues: FormValues = {
  cliente: "",
  cedulaCliente: "",
  direccion: "",
  sector: "",
  barrio: "",
  ciudad: "",
  departamento: "",

  departamentoId: "",
  ciudadId: "",

  fechaInforme: "",
  vetustez: "",
  fechaPagoAvaluo: "",
  fechaVisita: "",

  propietario: "",
  escritura: "",
  notaria: "",
  fechaEscritura: "",

  ciudadEscritura: "",
  departamentoEscritura: "",

  departamentoEscrituraId: "",
  ciudadEscrituraId: "",

  cedulaCatastral: "",
  licenciaConstruccion: "",
  nombreConjunto: "",
  coeficienteCopropiedad: "",

  distribucionInmueble: "",
  caracteristicasAdicionales: "",

  descripcion: "",
  garaje: "",
  descripcionGaraje: "",

  salaAreaConstruida: "",
  areaPrivada: "",

  inmueble: "",
};

export const requiredFields: Array<
  keyof FormValues
> = [
  "cliente",
  "cedulaCliente",
  "direccion",
  "sector",
  "barrio",
  "ciudad",
  "departamento",

  "fechaInforme",
  "vetustez",
  "fechaPagoAvaluo",
  "fechaVisita",

  "propietario",
  "escritura",
  "notaria",
  "fechaEscritura",
  "ciudadEscritura",
  "departamentoEscritura",
  "cedulaCatastral",
  "licenciaConstruccion",
  "nombreConjunto",
  "coeficienteCopropiedad",

  "distribucionInmueble",
  "caracteristicasAdicionales",

  "descripcion",
  "garaje",


  "salaAreaConstruida",
  "areaPrivada",

  "inmueble",
];

export const garageOptions = [
  {
    value: "SI",
    label: "SI",
  },
  {
    value: "NO",
    label: "NO",
  },
];