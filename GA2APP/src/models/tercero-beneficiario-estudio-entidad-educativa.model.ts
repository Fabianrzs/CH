export interface TerceroBeneficiarioEstudioEntidadEducativaModel {
    id:                     string;
    tipoDocEstudio:         string;
    tipoDocEstudioObj:      LlaveValor;
    numDocEstudio:          string;
    razonsocialEstudio:     string;
    parentescoEstudio:      string;
    parentescoEstudioObj:   LlaveValor;
    nit:                    string;
    razonSocial:       string;
    programaEntidad:      string;
    nivelEducativo:         string;
    reciboPago:             string;
    valor:                  string;
    fechalimite:            string;
}

interface LlaveValor {
    id:                     string,
    descripcion:            string
}