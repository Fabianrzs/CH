export interface Auditoria{
    id?:number;
    nombreTabla?:string;
    tipoAccion:string;
    nombreColumna?:string;
    valorAnterior?:string;
    valorNuevo?:string;
    fechaRegistro?:Date;
    usurioId:string;
    nombreUsuario:string;
    agrupador?:string;
    ip:any;
    sistemaOperativo:string;
    descripcion:string;
    navegador:string;
    idTipoAccion:number;
    hostname:string;
    modulo:string;
}


export interface LogAuditorias{    
    totalRegistros:number;
    numeroPaginas:number;
    auditorias:Auditoria[];
}


export interface PaginationTable{
    PageNum:number,
    Pagesize: number,
}


export interface TablasAuditoria{
    NombreTabla:string;
}

export interface ConsultaAuditoria{
    NombreTabla?:string;
    FechaInicio?:string;
    FechaFin?:string;
    UserId?:string;
    Agrupador?:string;
    VerDetalle:boolean;
    PageNum:number;
    PageSize:number;
    TipoAccion:string; 
    Usuario?:string;
    Descripcion?:string;
    Modulo?:string;
}

export interface TipoAccion{
    id:number;
    nombre:string;
}

export interface AuditoriaExcel{
    IdEvento:number;
    Fecha:string;    
    Accion:string;
    Descripcion:string;
    Modulo:string;
    Campo:string;
    ValorAnterior:string;
    ValorNuevo:string;
    Usuario:string;
    Hostname:string;
    Ip:string;
    Navegador:string;
    SistemaOperativo:string;      
}