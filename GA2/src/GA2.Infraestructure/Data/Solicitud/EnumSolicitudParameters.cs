using System.ComponentModel;


namespace GA2.Infraestructure.Data
{
    public enum EnumGeneralParameters
    {
        [Description("@TIP_SUB_ID")]
        SubModalidad,

        [Description("@SLT_ID")]
        SolicitudTarea,
    }

    public enum EnumSolicitudParameters
    {
        [Description("@SOL_ID")]
        IdSolicitud,

        [Description("@SOL_ID_FK")]
        SolicitudId,


        [Description("@Posee_Avac")]
        Posee_Avac,

        [Description("@CTA_ID")]
        cuentaId,

        [Description("@TPS_SUB_ID")]
        tipoSubModalidadId,

        [Description("@CLI_ID")]
        clienteId,

        [Description("@SOL_CREATEDOPOR")]
        creadoPor,

        [Description("@TRA_ID")]
        tareaId,

        [Description("@SOL_ESTADO_SOLICITUD")]
        solEstadoSolicitud,

        [Description("@STL_ESTADO_SOLICITUD")]
        stlEstadoSolicitud,

        [Description("@SOL_VALOR_A_RETIRAR")]
        valorRetirar,

        [Description("@OBSERVACION")]
        Observacion,

        [Description("@NUMERO_FOLIO")]
        Numero_Folio,

        [Description("@SOL_NUMERO_SOLICITUD")]
        Sol_Numero_Solicitud,

        [Description("@SOL_FECHACREACION")]
        Sol_fechaCreacion,

        [Description("@SOL_NUM_CEDULA")]
        Sol_Num_cedula,

        [Description("@SOL_NOMBRE_COMPLETO")]
        Sol_Nombre_Completo,

        [Description("@SOL_RADICADO")]
        Sol_radicado,

        [Description("@FECHA_FIRMA_ESCRITURA")]
        Fecha_firma_escritura,

        [Description("@INC_DESCRIPCION")]
        Inc_descripcion

    }

    public enum EnumMatriculaInmobiliaria
    {

        [Description("@MAI_ID")]
        MAI_ID,
        [Description("@MAI_ID_FK")]
        MAI_ID_FK
    }

    public enum EnumInsertarSolicitudInformacionAdjudicacion
    {
        [Description("@TipoVivienda")]
        TipoVivienda,
        [Description("@Direccion")]
        Direccion,
        [Description("@MatriculaInmobiliaria")]
        MatriculaInmobiliaria,
        [Description("@ValorSubsidio")]
        ValorSubsidio,
        [Description("@SOL_ID_FK")]
        SOL_ID_FK,
        [Description("@SIA_CREATEDOPOR")]
        SIE_CREATEDOPOR,
        [Description("@SOL_ID")]
        SOL_ID,
        [Description("@SIA_CIUDAD")]
        SIA_CIUDAD
    }



    public enum EnumInsertarSolicitudInformacionEscrituracion
    {
        [Description("@Notaria")]
        Notaria,
        [Description("@NumeroEscritura")]
        NumeroEscritura,
        [Description("@FechaEscrituracion")]
        FechaEscrituracion,
        [Description("@SOL_ID_FK")]
        SOL_ID_FK,
        [Description("@SIE_CREATEDOPOR")]
        SIE_CREATEDOPOR,
        [Description("@SOL_ID")]
        SOL_ID,
        [Description("@SIE_FECHACREACION")]
        FechaEcrituraRegistro,

    }

    public enum EnumConsultaSolicitudParameters
    {

        [Description("@TPS_SUB_ID")]
        tipoSubModalidadId,

        [Description("@CLI_ID")]
        clienteId,

        [Description("@SOL_ESTADO_ANULADO")]
        solEstadoAnulado,


        [Description("@CLI_IDENTIFICACION")]
        CLI_IDENTIFICACION

    }

    public enum EnumConsultaSolicitudPorActividadParameters
    {

        [Description("@SLT_ID")]
        sltId,
    }

    public enum EnumSolicitudMatriculaParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@NUMERO_MATRICULA")]
        NumeroMatricula,

        [Description("@FECHA_REGISTRO")]
        FechaRegistro,

        [Description("@DIRECCION")]
        Direccion,

        [Description("@DPC_ID_FK")]
        CiudadId,

        [Description("@MAI_PRINCIPAL")]
        Principal,

        [Description("@MAI_CREATEDOPOR")]
        CreadoPor,

        [Description("@DPD_ID_FK")]
        DepartamentoId,

        [Description("@OFC_ID")]
        OficinaRegistroId,

        [Description("@NUM_MATRICULA")]
        Num_matricula,

        [Description("@BLOQUEO_CREADO")]
        BloqueoCreado,

        [Description("@MAI_FECHA_BLOQUEO")]
        FechaBloqueoMatricula,

        [Description("@MAI_CONYUGE_ACTIVO")]
        ConyugeActivo

    }

    public enum EnumSolicitudPropietarioParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@NUMERO_IDENTIFICACION")]
        NumeroIdentificacion,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@PRP_CREATEDOPOR")]
        CreadoPor,

        [Description("@PRP_PRIMER_NOMBRE")]
        PrimerNombre,

        [Description("@PRP_SEGUNDO_NOMBRE")]
        SegundoNombre,

        [Description("@PRP_PRIMER_APELLIDO")]
        PrimerApellido,

        [Description("@PRP_SEGUNDO_APELLIDO")]
        SegundoApellido,

        [Description("@PRP_FECHACREACION")]
        FechaCreacion



    }

    public enum EnumSolicitudTerceroApoderadoParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTercero,

        [Description("@TER_ESAUTORIZADO")]
        EsAutorizado,

        [Description("@TER_PARENTESCO")]
        Parentesco,

        [Description("@TER_PRIMER_NOMBRE")]
        PrimerNombre,

        [Description("@TER_SEGUNDO_NOMBRE")]
        SegundoNombre,

        [Description("@TER_PRIMER_APELLIDO")]
        PrimerApellido,

        [Description("@TER_SEGUNDO_APELLIDO")]
        SegundoApellido,

        [Description("@TER_VALOR_RETIRAR")]
        ValorRetirarDos,

        [Description("@TER_FECHA_REGISTRO")]
        FechaRegistro

    }

    public enum EnumSolicitudTerceroBeneficiarioParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,
        
        [Description("@TER_ID")]
        TER_ID,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@ENT_ID_FK")]
        Entidad,

        [Description("@TER_TIPO_CUENTA_FK")]
        TipoCuenta,

        [Description("@NUMERO_CUENTA")]
        NumeroCuenta,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTercero,

        [Description("@TER_VALOR_RETIRAR")]
        ValorRetirarDos,

        [Description("@PVD_CODD")]
        CodProveeSeven,

        [Description("@TER_FECHACREACION")]
        FechaCreacionTer

    }

    public enum EnumSolicitudTerceroConstructorParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@NUMERO_LICENCIA_CONSTRUCCION")]
        NumeroLicencia,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTercero
    }

    public enum EnumConyugueParameters
    {
        [Description("@CLI_ID")]
        CliId,

        [Description("@CNY_TIPO")]
        CnyTipo,

        [Description("@CNY_IDENTIFICACION")]
        CnyIdentificacion,

        [Description("@CNY_PRIMER_NOMBRE")]
        CnyPrimerNombre,

        [Description("@CNY_SEGUNDO_NOMBRE")]
        CnySegundoNombre,

        [Description("@CNY_PRIMER_APELLIDO")]
        CnyPrimerApellido,

        [Description("@CNY_SEGUNDO_APELLIDO")]
        CnySegundoApellido
    }

    public enum EnumSolicitudTerceroVendedorParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@DIRECCION")]
        Direccion,

        [Description("@DPC_ID_FK")]
        Ciudad,

        [Description("@CORREO_ELECTRONICO")]
        CorreoElectronico,

        [Description("@TELEFONO")]
        Telefono,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTercero,

        [Description("@SIE_FECHACREACION")]
        FechaCreacionTer
    }

    public enum EnumInsertarArchivoParameters
    {
        [Description("@AST_ID")]
        ArchivoSolicitudId,

        [Description("@AST_NOMBRE_ARCHIVO")]
        NombreArchivo,

        [Description("@ASC_NOMBRE_ARCHIVO")]
        NombreArchivoCheck,

        [Description("@AST_RUTA_STORAGE")]
        RutaArchivo,

        [Description("@AST_EXTENSION")]
        ExtensionArchivo,

        [Description("@AST_FECHA_CARGUE")]
        FechaCargue,

        [Description("@SOL_ID_FK")]
        Solicitud,

        [Description("@PAM_ID_FK")]
        Parametrizacion,

        [Description("@PAM_ORDEN")]
        Orden,

        [Description("@AST_CREATEDOPOR")]
        CreadoPor,

        [Description("@AST_NUMERO_FOLIO")]
        NumeroFolio
    }

    public enum EnumActualizaSolicitudTareaParameters
    {
        [Description("@SLT_ID")]
        SolicitudTareaId,

        [Description("@EmbargoBloqueo")]
        EmbargoBloqueo,

        [Description("@NivelRiesgo")]
        NivelRiesgo,

        [Description("@SLT_ESTADO_SOLICITUD")]
        SolicitudTareaEstadoId,

        [Description("@SLT_ESTADO_SOLICITUD_NUEVO")]
        SolicitudTareaEstadoNuevoId,

        [Description("@SOL_ESTADO_SOLICITUD")]
        SolicitudEstadoId,

        [Description("@IdGrado")]
        IdGrado,

        [Description("@SLT_ESTADO_CLIENTE")]
        SolicitudTareaEstadoCliente,

        [Description("@SLT_ESTADO_CUENTA")]
        SolicitudEstadCcuenta,

        [Description("@FRZ_ID")]
        FRZ_ID,

        [Description("@NOMBRE_COMPLETO")]
        NOMBRE_COMPLETO,

        [Description("@NUM_CEDULA")]
        NUM_CEDULA,

        [Description("@SOL_ID")]
        SolicitudId,

        [Description("@NUMERO_RAD_SOLICITUD")]
        RadicadoSolicitudSG,

        [Description("@MPR_CONT")]
        Mpr_Cont,


        [Description("@SLT_OBSERVACION")]
        Observacion,

        [Description("@STL_FECHA_CREACION")]
        Fecha_tarea,

        [Description("@PRORRATEO")]
        Prorrateo,

        [Description("@REGLA_REGIMEN_CUOTA")]
        ReglaRegimenCuota,

        [Description("@SOL_RECHAZADA")]
        SolRechazada,

        [Description("@SOL_FECHA_REGISTRO")]
        FechaRegistro

    }

    public enum EnumConsultarSolicitudMatriculaParameters
    {
        [Description("@SOL_ID")]
        SolicitudId
    }

    public enum EnumSolicitudTerceroAutorizadoParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor
    }

    public enum EnumSolicitudPropietarioMatriculaParameters
    {
        [Description("@PRP_ID")]
        PropietarioId,

        [Description("@MAI_ID")]
        MatriculaId,

        [Description("@PMA_CREATEDOPOR")]
        CreadoPor,

        [Description("@@FECHA_PROPIETARIO")]
        FechaPropietario
    }

    public enum EnumSolicitudEntidadEducativaParameters
    {
        [Description("@PGN_ID_FK")]
        EntidadId,

        [Description("@TER_ID")]
        ID,

        [Description("@NUMERO_DOCUMENTO")]
        Nit,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@PRN_ID_FK")]
        NivelId,

        [Description("@TER_NUM_RECIBO_PAGO")]
        ReciboPago,

        [Description("@TER_FECHA_LIMITE_PAGO")]
        FechaPago,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTerceroEntidadEducativa,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        TipoIdentificacionNit,

        [Description("@TER_ENT_DEPARTAMENTO")]
        TER_ENT_DEPARTAMENTO,

        [Description("@TER_ENT_CIUDAD")]
        TER_ENT_CIUDAD,

        [Description("@TER_ENT_MODALIDAD_ESTUDIO")]
        TER_ENT_MODALIDAD_ESTUDIO,

        [Description("@TER_ENT_PERIODO_ACADEMICO")]
        TER_ENT_PERIODO_ACADEMICO,

        [Description("@TER_ENT_HORAS")]
        TER_ENT_HORAS,

        [Description("@TER_ENT_FECHA_INICIO")]
        TER_ENT_FECHA_INICIO,

        [Description("@TER_ENT_FECHA_FIN")]
        TER_ENT_FECHA_FIN,

        [Description("@TER_ENT_ACREDITADA")]
        TER_ENT_ACREDITADA,

        [Description("@TER_ENT_VALOR_RECIBO")]
        TER_ENT_VALOR_RECIBO

    }

    public enum EnumSolicitudTerceroBeneficiarioEstudioParameters
    {
        [Description("@TER_NUMERO_DOCUMENTO")]
        numDoc,

        [Description("@TER_NOMBRE_RAZON_SOCIAL")]
        razonSocialEstudiante,

        [Description("@TER_PARENTESCO")]
        parentescoId,

        [Description("@CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTerceroBeneficiarioEstudioId,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        tipoDocId
    }

    public enum EnumSolicitudTerceroBeneficiarioEstudioEntidadEducativaParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTerceroBeneficiarioEstudioEntidadEducativaId,

        [Description("@TID_ID_FK")]
        tipoDocId,

        [Description("@TER_NUMERO_DOCUMENTO")]
        numDoc,

        [Description("@TER_NOMBRE_RAZON_SOCIAL")]
        razonSocialEstudiante,

        [Description("@TER_PARENTESCO")]
        parentescoId,

        [Description("@PGN_ID_FK")]
        EntidadId,

        [Description("@NUMERO_DOCUMENTO")]
        Nit,

        [Description("@NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@PRN_ID_FK")]
        NivelId,

        [Description("@TER_NUM_RECIBO_PAGO")]
        ReciboPago,

        [Description("@SOL_VALOR_A_RETIRAR")]
        valorRetirar,

        [Description("@TER_FECHA_LIMITE_PAGO")]
        FechaPago,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor
    }

    public enum EnumSolicitudTerceroEntidadSeguroEducativoParameters
    {
        [Description("@TER_NUMERO_DOCUMENTO")]
        numDoc,

        [Description("@TER_NOMBRE_RAZON_SOCIAL")]
        razonSocialAseguradora,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTerceroEntidadSeguroId,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        tipoDocId,

        [Description("@ESE_ID_FK")]
        entidadSeguroEducativoId,

        [Description("@TER_NUM_POLIZA")]
        numeroPoliza
    }

    public enum EnumSolicitudTerceroTenedorAccionesParameters
    {
        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@TER_NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@TER_NOMBRE_RAZON_SOCIAL")]
        RazonSocial,

        [Description("@TER_DIRECCION")]
        Direccion,

        [Description("@DPC_ID_FK")]
        Ciudad,

        [Description("@TER_CORREO_ELECTRONICO")]
        CorreoElectronico,

        [Description("@TER_TELEFONO")]
        Telefono,

        [Description("@TER_CREATEDOPOR")]
        CreadoPor,

        [Description("@TER_TIPO_TERCERO_FK")]
        TipoTercero,

        [Description("@TER_EMISOR_NOMBRE_RAZON_SOCIAL")]
        RazonSocialEmisor
    }

    public enum EnumInconsistenciaSolicitudParameters
    {

        [Description("@INC_ID")]
        InconsistenciaId,

        [Description("@LIN_ID_FK")]
        ListaInconsistenciaId,

        [Description("@NVD_ID_FK")]
        Novedad,

        [Description("@GIN_ID_FK")]
        GrupoInconsistencia,

        [Description("@HER_ID_FK")]
        HerramientaId,

        [Description("@PTA_ID_FK")]
        PuntoAtencionId,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@USR_ID_FK")]
        TecnicoId,

        [Description("@INC_OBJETO_ESTUDIO")]
        ObjetoEstudio,

        [Description("@INC_ANALISIS_REALIZADO")]
        Analisis,

        [Description("@INC_OBSERVACION")]
        Observacion,

        [Description("@INC_ESTADO")]
        Estado,

        [Description("@INC_CREATEDOPOR")]
        CreadoPor,

        [Description("@INC_MODIFICADOPOR")]
        ModificadoPor,

        [Description("@INC_FECHAMODIFICACION")]
        FechaModificacion,

        [Description("@CMN_ID")]
        IdComunicacion,

        [Description("@CSC_ID")]
        IdCausalComunicacion,

        [Description("@CIN_ID")]
        IdCausalInterrupcion,

        [Description("@CMN_TEXTO_INCLUIR")]
        TextoIncluir,

        [Description("@CMN_FIRMA_DIGITAL")]
        FirmaDigital,

       [Description("@CMN_RADICADO")]
        Radicado,

        [Description("@CMN_RADICADO_RELACIONADO")]
        RadicadoRelacionado
    }

    public enum EnumConsultarSolicitudCesantiasParameters
    {


        [Description("@SLT_ESTADO_SOLICITUD")]
        EstadoSolicitud,

        [Description("@PCS_ID")]
        Identificacion,

        [Description("@NumRadicado")]
        NumRadicado,

        [Description("@FechaIni")]
        FechaIni,

        [Description("@FechaFin")]
        FechaFin,

        [Description("@Tipo")]
        Tipo,

        [Description("@Modalidad")]
        Modalidad,

        [Description("@SubModalidad")]
        SubModalidad,

        [Description("@Usuario")]
        Usuario,

        [Description("@NumeroSolicitud")]
        NumeroSolicitud,

        [Description("@UsuarioIngreso")]
        UsuarioIngreso,

        [Description("@Solicitante")]
        Solicitante
    }

    public enum EnumObtenerFomulariosSolicitudParameters
    {
        [Description("@TIM_CODIGO")]
        TimCodigo,
    }
    public enum EnumOCrearConvocatoriaParameters
    {
        [Description("@ID_CONVOCATORIA")]
        IdConvocatoria,

        [Description("@FECHA_APERTURA")]
        FechaApertura,

        [Description("@FECHA_CIERRE")]
        FechaCierre,

        [Description("@FECHA_PUBLICACION")]
        FechaPublicacion,

        [Description("@NUM_BENEFICIARIO")]
        NumBeneficiarion,

        [Description("@NUM_RESOLUCION")]
        NumResolucion,

        [Description("@NUM_CONVOCATORIA")]
        NumConvocatoria,

        [Description("@NUM_DIARIO_OFICIAL")]
        NumDiarioOficial,

        [Description("@ESTADO_CONVOCATORIA_ID")]
        EstadoConvocatoriaId,
        [Description("@FECHARESOLUCION")]
        FechaResolucion,
    }
    public enum EnumConsultarConvocatoria
    {
        [Description("@NO_CONVOCATORIA")]
        NoConvocatoria,

        [Description("@ESTADO_CONVOCATORIA_ID")]
        EstadoId,

        [Description("@FECHA_INICIO")]
        FechaInicio,

        [Description("@FECHA_FIN")]
        FechaFin,

        [Description("@FECHA_PUBLICACION")]
        FechaPublicacion,
    }
    public enum EnumConsultarTipoSolicitudPostulante
    {
        [Description("@ID_SOLICITUD")]
        IdSolicitud,

    }
    public enum EnumConsultarTipoSubModalidadPostulante
    {
        [Description("ID_MODALIDAD")]
        IdSolicitud,

    }
    public enum EnumConsultarTipoModalidadPostulante
    {
        [Description("@MPO_ID")]
        Mpo_Id,

    }
    public enum EnumConsultarSolicitudPostulacionById
    {
        [Description("identificacion")]
        Identificacion,
        [Description("id")]
        Id,

    }
    public enum SpoCrearSolicitudPostulante
    {
        [Description("SPO_TIPO_DOCUMENTO_ID")]
        TipoDocumentoId,

        [Description("SPO_NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("SPO_ID_TIPO_DE_SOLICITUD")]
        IdTipoSolicitud,

        [Description("SPO_ID_MODALIDAD")]
        IdMdalidad,

        [Description("SPO_ID_SUBMODALIDAD")]
        IdSubModalidad,

        [Description("SPO_MANIFESTACION_AUTORIZACIONES")]
        ManiestacionAutorizaciones,

        [Description("SPO_DEPARTAMENTO_PREFERENCIA1")]
        Deparmaento1,

        [Description("SPO_CIUDAD_PREFERENCIA1")]
        Ciudad1,

        [Description("SPO_DEPARTAMENTO_PREFERENCIA2")]
        Deparmaento2,

        [Description("SPO_CIUDAD_PREFERENCIA2")]
        Ciudad2,

        [Description("SPO_DEPARTAMENTO_PREFERENCIA3")]
        Deparmaento3,

        [Description("SPO_CIUDAD_PREFERENCIA3")]
        Ciudad3,

        [Description("SPO_TIPO_DOCUMENTO_FIRMA_SOLICITUD")]
        FirmaSolicitud,

        [Description("SPO_NUMERO_DOCUMENTO_FIRMA_SOLICITUD")]
        NumeroDocumentoFirma,

        [Description("SPO_FECHAEXPEDICION")]
        FechaExpedicion,

        [Description("SPO_NUMERO_CELULAR")]
        NumeroCelular,

        [Description("SPO_NUMERO_CONVOCATORIA")]
        NumeroConvocatoria,

        [Description("SPO_ESTADO")]
        Estado,

        [Description("@SPO_ACTIVIDAD")]
        Actividad,

        [Description("SPO_NOMBRES_FIRMA")]
        NombreFirma,

        [Description("SPO_APELLIDOS_FIRMA")]
        ApellidoFirma,

        [Description("@SPO_OBSERVACION")]
        Observacion,

        [Description("@SPO_FECHA_CREACION")]
        FechaCreacion,

        [Description("@SPO_REPRESENTANTE_LEGAL")]
        RepresentanteLegal,

        [Description("@SPO_DISFRUTE_PENSION")]
        SPO_DISFRUTE_PENSION,

        [Description("@SPO_UNIDAD_EJECUTORA")]
        SPO_UNIDAD_EJECUTORA,

        [Description("@SPO_CATEGORIA")]
        SPO_CATEGORIA,

        [Description("@SPO_NOMBRE_AFILIADO")]
        SPO_NOMBRE_AFILIADO
            ,
        [Description("@SPO_USERID")]
        SPO_USERID,

        [Description("@ModalidadSolucionVivienda")]
        ModalidadSolucionVivienda,

        [Description("@ValorAfectacion")]
        ValorAfectacion,

        [Description("@AfectacionCuenta")]
        AfectacionCuenta,

        [Description("@Fuerza")]
        Fuerza,


        [Description("@IdConvocatoria")]
        IdConvocatoria,
    }

    public enum CrearPostulante
    {
        [Description("POS_ID_SOLICIUD")]
        IdSolicitud,

        [Description("POS_NOMBRE")]
        Nombre,

        [Description("POS_APELLIDO")]
        Apellido,

        [Description("POS_ID_TIPO_DOCUMENTO")]
        TipoDocumentoId,

        [Description("POS_NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("POS_DEPARMENTO_EXPEDICION")]
        DepamentoExpedicion,

        [Description("POS_CIUDAD_EXPEDICION")]
        CiduadExpedicion,

        [Description("POS_PARENTESCO")]
        Parentesco,

        [Description("POS_SEXO")]
        Sexo,

        [Description("POS_FECHA_NACIMIENTO")]
        FechaNacimiento,

        [Description("POS_ESTADO_CIVIL")]
        EstadoCivil,

        [Description("POS_OCUPACION")]
        Ocupacion,

        [Description("POS_NIVEL_EDUCATIVO")]
        NivelEducativo,

        [Description("POS_AFECTACION_FISICA")]
        AfectacionFisica,

        [Description("POS_AFECTACIONSENSORIAL")]
        AfectacionSensorial,

        [Description("POS_ENFERMEDADESPECIFICA")]
        EnfermedadesEspecificas,

        [Description("POS_TRATAMIENTOMEDICO")]
        TratamientoMedico,

        [Description("POS_DIFICULTADESCALERAS")]
        DificualtaEscaleras,

        [Description("POS_MEDICAMENTOPERMANENTE")]
        MedicamentoPermanente,

        [Description("POS_AYUDATECNICASENSORIAL")]
        AyudaSensorial,

        [Description("POS_CONDICIONSALUD")]
        CondicionSalud,

        [Description("TipoIntegranteId")]
        TipoIntegranteId,

        [Description("TipoParentescoId")]
        TipoParentescoId,

        [Description("TipAyudaTecnicaId")]
        TipAyudaTecnicaId,

        [Description("TipoDiscapacidadId")]
        TipoDiscapacidadId,

        [Description("PorcentajePostulacion")]
        PorcentajePostulacion,

        [Description("PorcentajeAportes")]
        PorcentajeAportes,

        [Description("PorcentajeCesantias")]
        PorcentajeCesantias,

    }
    public enum EnumObtenerPostulateByNumeroDocumento
    {
        [Description("@numeroIdentificacion")]
        NumeroDocumento,

    }
    public enum EnumUpdateSolicitudPostulacion
    {
        [Description("@IdSolicitud")]
        Id,

        [Description("@manifestacionAutorizacion")]
        ManifesracionAutorizacion,

        [Description("@department1")]
        Deparmaento1,

        [Description("@city1")]
        Ciudad1,

        [Description("@deparment2")]
        Deparmaento2,

        [Description("@city2")]
        Ciudad2,

        [Description("@deparment3")]
        Deparmaento3,

        [Description("@city3")]
        Ciudad3,

        [Description("@tipoDocumentoFirma")]
        TipoDocumentoFimar,

        [Description("@numDocumentoFirma")]
        NumeroDocumentoFimar,

        [Description("@fechaExpedicion")]
        FechaExpeidcion,

        [Description("@numeroCelular")]
        NumeroCelular,
        [Description("@Observacion")]
        Observacion,

    }
    public enum EnumUpdatePostulante
    {
        [Description("@IdSolicitud")]
        Id,

        [Description("@nombre")]
        Nombre,

        [Description("@apellido")]
        Apellido,

        [Description("@tipoDocumento")]
        TipoDocumento,

        [Description("@numeroDocumento")]
        NumeroDocumento,

        [Description("@departmento")]
        Deparmaento,

        [Description("@city")]
        Ciudad,

        [Description("@parentesco")]
        Parenteso,

        [Description("@sexo")]
        Sexo,

        [Description("@fechaNacimiento")]
        FechaNacimiento,

        [Description("@estadiCivil")]
        EstadoCivil,

        [Description("@ocupaion")]
        Ocupacion,

        [Description("@nivelEducadivo")]
        NivelEducativo,

        [Description("@afectacionFisica")]
        AfectacionFisica,

        [Description("@afectacionSensorial")]
        AfectacionSensorial,

        [Description("@enfermedadEspecifica")]
        EnfermeadEspecifica,

        [Description("@tratamientoMedico")]
        TratamientoMedico,

        [Description("@dificultadEscalera")]
        DificultaEscaleras,

        [Description("@medicamentoPermanente")]
        MedicamentoPermanente,

        [Description("@ayudaTecnica")]
        AyudaTecnica,

        [Description("@condicionSalud")]
        CondicionSalud,
    }
    public enum EnumUpdateStatusAproved
    {
        [Description("@IdSolicitud")]
        Id,

        [Description("@EstadoNombre")]
        Nombre,

        [Description("@Actividad")]
        Actividad,
    }
    public enum EnumConsultarListaArchivoPostulanteByModaladad
    {
        [Description("TIP_SUB_ID")]
        Id,
        [Description("SLT_ID")]
        IdSolicitud,
    }
    public enum InsertarArchivoPostulacion
    {
        [Description("@Nombre_Archivo")]
        Nombre_Archivo,
        [Description("@Ruta_Storage")]
        Ruta_Storage,
        [Description("@Extension")]
        Extension,
        [Description("@Solicitud_Id")]
        Solicitud_Id,
        [Description("@Numero_Folio")]
        Numero_Folio,
        [Description("@Archivo_Id")]
        Archivo_Id,
        [Description("@UserId")]
        UserId,

    }
    public enum EnumsEliminarArchivoPorSolicitud
    {
        [Description("@Asp_Id")]
        Asp_Id,

    }
    public enum EnumsConsultarArchivoPostulacionById
    {
        [Description("@SOL_ID")]
        Sol_Id,

    }
    public enum EnumsActualizarEstadoYActividadArchivo
    {
        [Description("@IdSolicitud")]
        Sol_Id,
        [Description("@Estado")]
        Estado,
        [Description("@Actividad")]
        Actividad,
        [Description("@UserId")]
        UserId,
        [Description("@DateCreate")]
        DateCreate,
        [Description("@N_Folio")]
        N_Folio,

    }

    public enum EnumsCrearEntrevista
    {
        [Description("@EFS_ID")]
        Id,
        [Description("@EFS_ID_SOLICITUD")]
        IdSolicitud,
        [Description("@EFS_TIPO_BENEFICIO")]
        TipoBeneficio,
        [Description("@EFS_VIVIENDA_PROPIA")]
        ViviendaPropia,
        [Description("@EFS_TIPO_VIVIENDA")]
        TipoVivienda,
        [Description("@EFS_HABITANTE")]
        NoHabitante,
        [Description("@EFS_CAUSAL")]
        Causal,
        [Description("@EFS_INFORMATIVO_ADMIN")]
        InforAdmi,
        [Description("@EFS_CARACTERIZACION")]
        Caracterizacion,
        [Description("@EFS_CONCEPTO_PSICOSOCIAL")]
        ConceptoPsicosocial,
        [Description("@EFS_OBSERVACION")]
        Observacion,
        [Description("@EFS_ESTADO")]
        Estado,
        [Description("@EFS_ACTIVIDAD")]
        Actividad,
        [Description("@UserId")]
        UserId,

    }
    public enum EnumsCrearClasificacionFs
    {
        [Description("@IdCalificacion")]
        IdCalificacion,
        [Description("@SolicitudId")]
        Sol_Id,
        [Description("@UnidadEjecutora")]
        UnidadEjecutora,
        [Description("@TipoImputabilidad")]
        TipoImputabilidad,
        [Description("@ImputabilidadAlServicio")]
        ImputabilidadAlServicio,
        [Description("@PorcentajePostulacion")]
        PorcentajePostulacion,
        [Description("@Hecho")]
        Hecho,
        [Description("@Puntaje")]
        Puntaje,
        [Description("@DisfrutePension")]
        DisfrutePension,
        [Description("@DatosResolucion")]
        DatosResolucion,
        [Description("@EstadoId")]
        Estado,
        [Description("@Actividad")]
        Actividad,
        [Description("@Observacion")]
        Observacion,
        [Description("@HechoDescripcion")]
        HechoDescripcion,
        [Description("@UserId")]
        UserId,
        [Description("@PorcentajeDisminucion")]
        PorcentajeDisminucion,
    }
    public enum EnumsActualizarObservacion
    {
        [Description("@SolicitudId")]
        Sol_Id,
        [Description("@Observacion")]
        Observacion,
    }
    public enum EnumConsultarSolicitudPostulacion
    {
        [Description("identificacion")]
        Identificacion,
        [Description("EtapasId")]
        EtapasId,
        [Description("TipoSolicitudId")]
        TipoSolicitudId,
        [Description("NoConvocatoria")]
        NoConvocatoria,
        [Description("NoRadicado")]
        NoRadicado,
        [Description("UserId")]
        UserId,

    }
    public enum EnumsEliminarConvocatoria
    {
        [Description("@IdConvocatoria")]
        IdConvocatoria,

        [Description("@Delete")]
        Delete,

        [Description("@EstadoConvocatoria")]
        EstadoConvocatoria

    }
    public enum EnumsActualizarEstadoRadicado
    {
        [Description("@IdSolicitud")]
        Sol_Id,
        [Description("@Radicado")]
        Radicado,

    }

    public enum EnumsPaginacion
    {
        [Description("@PageNumber")]
        PageNumber,
        [Description("@PageSize")]
        PageSize,

    }

    public enum EnumsCreateVerificatePostulacion
    {
        [Description("@IdSolicitud")]
        Sol_Id,
        [Description("@EtapaId")]
        EtapaId,
        [Description("@UserId")]
        UserId,
        [Description("@DateCreate")]
        DateCreate,
    }
    public enum EnumsUpdateVerificatePostulacion
    {
        [Description("@IdSolicitud")]
        Sol_Id,
        [Description("@Observacion")]
        Observacion,
        [Description("@EtapaId")]
        EtapaId,
    }
    public enum EnumsUpdateStatusPostulacion
    {
        [Description("@IdSolicitud")]
        Sol_Id,
        [Description("@StatusId")]
        StatusId,
        [Description("@EtapaId")]
        EtapaId,
    }

    public enum EnumsCreateTrazabilidadFondo
    {
        [Description("@IdSolicitud")]
        Sol_Id,
        [Description("@UserName")]
        UserName,
        [Description("@Observation")]
        Observation,
        [Description("@ActivityId")]
        ActivityId,
        [Description("@EstadoId")]
        EstadoId,
        [Description("@DateCreate")]
        DateCreate,
    }

    public enum EnumInformacionTurno
    {
        [Description("@SIT_ID")]
        SitId,
        [Description("@RIT_ID_FK")]
        RitIdFk,
        [Description("@SIT_ID_FK")]
        SitIdFk,
        [Description("@TIT_ID_FK")]
        TitIdFk,
        [Description("@SIT_PRE_TRAMITE")]
        SitPreTramite,
        [Description("@SIT_TURNO_NUMERO")]
        SitTurnoNumero,
        [Description("@SIT_NUMERO_COMISION")]
        SitNumeroComision,
        [Description("@SIT_FECHA_COMISION")]
        SitFechaComision,
        [Description("@SOL_ID_FK")]
        SolIdFk
    }
    public enum EnumUpdateFechaCiiere
    {
        [Description("@FechaPublicacionDiaro")]
        FechaPublicacionDiaro,
        [Description("@FechaNotificacion")]
        FechaNotificacion,
        [Description("@IdConvocatoria")]
        IdConvocatoria,
    }

    public enum EnumFactSolicitud
    {
        [Description("@FAC_CONT")]
        factCont,
        [Description("@FAC_NUME")]
        factNume
    }

    public enum EnumInformacionClienteSolicitud
    {
        [Description("@SOL_CEDULA")]
        CedulaCliente
    }

    public enum EnumInconsistenciaSeguridadSolicitudParameters
    {

        [Description("@INC_INCONSISTENCIA")]
        Inconsistencia,

        [Description("@INC_ESTADO")]
        Estado,

        [Description("@INC_GRUPO")]
        Grupo,

        [Description("@INC_SUBSANADO")]
        Subsanado,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@USR_ID_FK")]
        UserId,

        [Description("@TPO_NOV_ID")]
        TipoNovedadId,

        [Description("@CAU_ID")]
        CausalInterrupcionId,

        [Description("@INF_ID")]
        InformeInconsistenciaId,

        [Description("@TPO_NOV_ID_NUEVO")]
        NovedadNuevaId,

        [Description("@CAU_ID_NUEVO")]
        CausalNuevaId,

        [Description("@INF_ID_NUEVO")]
        Informe114NuevaId,

        [Description("@FECHA_INC")]
        Fecha_Inconsistencia,

        [Description("@FECHA_SUBSANACION")]
        Fecha_Subsanacion,

        [Description("@FECHA_REGISTRO")]
        FECHA_REGISTRO,

        [Description("@SLT_ID")]
        SolicitudTareaId

    }

    public enum EnumConceptoSolicitud
    {
        [Description("@DESCRIPCION_CUENTA")]
        TipoCuenta,

        [Description("@CTC_ID")]
        ConceptoID,

        [Description("@CTC_DESCRIPCION")]
        ConceptoDescripcion,

        [Description("@SOL_ID_FK")]
        SolicitudID,

        [Description("@CLI_ID")]
        ClienteID,

        [Description("@CLI_IDENTIFICACION")]
        IdentificacionCliente,

        [Description("@CAC_SALDO")]
        SaldoCuenta,

        [Description("@CAC_VALOR_A_RETIRAR")]
        ValorRetirar,

        [Description("@NUMERO_CUENTA")]
        NumeroCuenta,

        [Description("@CAC_VALOR_DESCONTADO")]
        ValorDescontadoConcepto,

        [Description("@TER_TIPO_CUENTA")]
        TipoCuentaCatalogo,

        [Description("@ENT_ID_FK")]
        EntidadBancaria

    }

    public enum EnumSolicitudSubModalidadesParameters
    {
        [Description("@TPS_SUB_ID")]
        SubmodalidadId
    }

    public enum EnumSolicitudReglaAsignacionSubsidio
    {
        [Description("@NUMERO_MATRICULA")]
        NumeroMatricula
    }

    public enum EnumSolicitudBuscarUsuario
    {
        [Description("@NOMBRES")]
        NombresUsuario,

        [Description("@APELLIDOS")]
        ApellidoUsuario
    }


    public enum EnumSolicitudBuscarSolicitante
    {
        [Description("@NOMBRES")]
        NombresSolicitante,

        [Description("@APELLIDOS")]
        ApellidSolicitante
    }

    public enum EnumIntervinientesActoresSolicitud
    {
        [Description("@ITR_ID")]
        IdInterviniente,

        [Description("@MODULO")]
        Modulo,

        [Description("@ITR_MODIFICADOPOR")]
        ModificadoPor,

        [Description("@SOL_NUMERO_SOLICITUD")]
        NumSolicitud,

        [Description("@SOL_RADICADO_GD")]
        Radicado,

        [Description("@SOL_ID_FK")]
        SolId,

        [Description("@ITR_TIPO_PERSONA")]
        TipoPersona,

        [Description("@ITR_TIPO_DOCUMENTO")]
        TipoDocumento,

        [Description("@ITR_NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@ITR_NOMBRE")]
        NombreRazonSocial,

        [Description("@ITR_SEGUNDONOMBRE")]
        SegundoNombre,

        [Description("@ITR_PRIMERAPELLIDO")]
        PrimerApellido,

        [Description("@ITR_SEGUNDOAPELLIDO")]
        SegundoApellido,

        [Description("@ITR_CELULAR")]
        Celular,

        [Description("@ITR_CORREO_ELECTRONICO")]
        CorreoElectronico,

        [Description("@ITR_DEPARTAMENTO")]
        DepartamentoId,

        [Description("@ITR_CIUDAD")]
        CiudadId,

        [Description("@ITR_DIRECCION")]
        Direccion,

        [Description("@ITR_BARRIO")]
        Barrio,

        [Description("@ITR_TELEFONO")]
        Telefono,

        [Description("@ITA_ID_FK")]
        ActorId,

        [Description("@ITR_TIPO_CUENTA")]
        TipoCuenta,

        [Description("@ITR_NUMERO_CUENTA_BANCARIA")]
        NumeroCuenta,

        [Description("@ITR_ENTIDAD_BANCARIA")]
        EntidadBancaria,

        [Description("@ITR_FECHA_EXPEDICION")]
        FechaExpedicion,

        [Description("@ITR_CONSTRUCTORA")]
        Constructora,

        [Description("@ITR_CONVENIO_ENTIDAD")]
        Convenio,

        [Description("@ITR_TIPO_DOCUMENTO_AFILIADO")]
        TipoDocumentoAfiliado,

        [Description("@ITR_NUMERO_DOCUMENTO_AFILIADO")]
        NumeroDocumentoAfiliado

    }

    public enum EnumSolicitudTerceroGeneral
    {
        [Description("@SLT_ID")]
        SolicitudTareaId
    }
    public enum EnumSolicitudEstados
    {
        [Description("@CVL_DESCRIPCION")]
        DescripcionEstado
    }

    public enum EnumPropietarioMatriculaSolicitud
    {
        [Description("@NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@PRP_PRIMER_NOMBRE")]
        PrimerNombrePropietario,

        [Description("@PRP_SEGUNDO_NOMBRE")]
        SegundoNombrePropietario,

        [Description("@PRP_PRIMER_APELLIDO")]
        PrimerApellidoPropietario,

        [Description("@PRP_SEGUNDO_APELLIDO")]
        SegundoApellidoPropietario,

    }

    public enum EnumSolicitudCuenta
    {
        [Description("@SOL_ID")]
        SolicitudId,

        [Description("@SALDO_CUENTA")]
        SaldoCuenta,

        [Description("@CUENTA")]
        Cuenta,

        [Description("@TRASLADO_AUTORIZADO")]
        TrasladoAutorizado,

    }
}