export type FormValues = {
    cliente: string;
    cedulaCliente: string;
    direccion: string;
    sector: string;
    barrio: string;
    ciudad: string;
    departamento: string;
  
    departamentoId: string;
    ciudadId: string;
  
    fechaInforme: string;
    vetustez: string;
    fechaPagoAvaluo: string;
    fechaVisita: string;
  
    propietario: string;
    escritura: string;
    notaria: string;
    fechaEscritura: string;
  
    ciudadEscritura: string;
    departamentoEscritura: string;
  
    departamentoEscrituraId: string;
    ciudadEscrituraId: string;
  
    cedulaCatastral: string;
    licenciaConstruccion: string;
    nombreConjunto: string;
    coeficienteCopropiedad: string;
  
    distribucionInmueble: string;
    caracteristicasAdicionales: string;
  
    descripcion: string;
    garaje: string;
    descripcionGaraje: string;
  
    salaAreaConstruida: string;
    areaPrivada: string;
  
    inmueble: string;
  };
  
  export type FormErrors = Partial<
    Record<keyof FormValues, string>
  >;
  
  export type SelectOption = {
    value: string;
    label: string;
  };
  
  export type AvaluoSectionProps = {
    values: FormValues;
    errors: FormErrors;
    onChange: (
      field: keyof FormValues,
      value: string
    ) => void;
  };
  
  export type AvaluoComercialProps = {
    procedureRequest: any;
    isLoading: boolean;
  };