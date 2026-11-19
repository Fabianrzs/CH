/*
  Base de datos STUB para la prueba de humo GA2 → MockIntegrations.
  NO es la base de GA2: sólo contiene los procedimientos que GA2 ejecuta al arrancar y en los
  endpoints de la prueba (interruptores de integración y auditoría de llamadas). Sirve para
  comprobar el cableado de configuración sin depender de la base real (GA2_DEVELOP).
  Firmas tomadas de: CatalogosRepository (Hangfire), EstadoIntegracionRepository / CreditoRepository
  (ObtenerParametrosIntegraciones) y AuditoriaRepository (InsertReportIntegraciones*, UpdateReportIntegraciones).
*/
IF DB_ID('GA2_MOCKS_SMOKE') IS NULL CREATE DATABASE GA2_MOCKS_SMOKE;
GO
USE GA2_MOCKS_SMOKE;
GO

-- Arranque: HangfireJobsInitializer.RegistrarOActualizarJobsAsync (job inactivo).
CREATE OR ALTER PROCEDURE dbo.ObtenerParametrizacionHangFire AS
  SELECT CAST(1 AS int) AS CVL_ID, 'MOCK_JOB' AS CVL_CODIGO, '0 6 * * *' AS CVL_DESCRIPCION, 'False' AS CVL_ACTIVO;
GO
CREATE OR ALTER PROCEDURE dbo.InsertarLogHangFire @HL_MENSAJE nvarchar(max) = NULL, @HL_REQUEST_FAIL nvarchar(max) = NULL AS
  SELECT 1;
GO

-- Interruptores de integración (dbo.ObtenerParametrosIntegraciones → {PARAM_ID, INTEGRACION, PARAMETRO, VALOR}).
IF OBJECT_ID('dbo.MOCK_PARAMETROS_INTEGRACIONES') IS NULL
  CREATE TABLE dbo.MOCK_PARAMETROS_INTEGRACIONES (
    PARAM_ID uniqueidentifier NOT NULL DEFAULT NEWID() PRIMARY KEY,
    INTEGRACION nvarchar(50) NOT NULL,
    PARAMETRO nvarchar(50) NOT NULL,
    VALOR nvarchar(200) NULL);
GO
DELETE FROM dbo.MOCK_PARAMETROS_INTEGRACIONES;
INSERT INTO dbo.MOCK_PARAMETROS_INTEGRACIONES (INTEGRACION, PARAMETRO, VALOR) VALUES
  ('VIGIA', 'ESTADO', 'true'), ('FENIX', 'ESTADO', 'true'), ('DODO', 'ESTADO', 'true'),
  ('TusDatos', 'ESTADO', 'true'), ('Bua', 'ESTADO', 'true');
GO
CREATE OR ALTER PROCEDURE dbo.ObtenerParametrosIntegraciones AS
  SELECT PARAM_ID, INTEGRACION, PARAMETRO, VALOR FROM dbo.MOCK_PARAMETROS_INTEGRACIONES;
GO

-- Auditoría de llamadas (queda como evidencia de lo que GA2 envió y recibió).
IF OBJECT_ID('dbo.MOCK_REPORT_INTEGRACIONES') IS NULL
  CREATE TABLE dbo.MOCK_REPORT_INTEGRACIONES (
    ID_INTEGRACIONES uniqueidentifier NOT NULL PRIMARY KEY,
    TIPOSERVICIO nvarchar(100) NULL, METODOHTTP nvarchar(20) NULL, ENDPOINT nvarchar(1000) NULL,
    REQUEST nvarchar(max) NULL, RESPONSE nvarchar(max) NULL, STATUSCODE int NULL, SUCCESS bit NULL,
    ERROR nvarchar(max) NULL, FECHAINICIO datetime2 NULL, FECHAFIN datetime2 NULL, DURACIONMS bigint NULL,
    REGISTRADO datetime2 NOT NULL DEFAULT SYSDATETIME());
GO
CREATE OR ALTER PROCEDURE dbo.InsertReportIntegraciones
  @ID_INTEGRACIONES uniqueidentifier, @REQUEST nvarchar(max) = NULL, @TIPOSERVICIO nvarchar(100) = NULL AS
BEGIN
  INSERT INTO dbo.MOCK_REPORT_INTEGRACIONES (ID_INTEGRACIONES, TIPOSERVICIO, REQUEST) VALUES (@ID_INTEGRACIONES, @TIPOSERVICIO, @REQUEST);
  SELECT CAST(1 AS bit);
END
GO
CREATE OR ALTER PROCEDURE dbo.UpdateReportIntegraciones
  @ID_INTEGRACIONES uniqueidentifier, @RESPONSE nvarchar(max) = NULL AS
BEGIN
  UPDATE dbo.MOCK_REPORT_INTEGRACIONES SET RESPONSE = @RESPONSE, FECHAFIN = SYSDATETIME() WHERE ID_INTEGRACIONES = @ID_INTEGRACIONES;
  SELECT CAST(1 AS bit);
END
GO
CREATE OR ALTER PROCEDURE dbo.InsertReportIntegracionesRest
  @ID_INTEGRACIONES uniqueidentifier, @TIPOSERVICIO nvarchar(100) = NULL, @METODOHTTP nvarchar(20) = NULL,
  @ENDPOINT nvarchar(1000) = NULL, @REQUEST nvarchar(max) = NULL, @RESPONSE nvarchar(max) = NULL,
  @STATUSCODE int = NULL, @SUCCESS bit = NULL, @ERROR nvarchar(max) = NULL, @FECHAINICIO datetime2 = NULL,
  @FECHAFIN datetime2 = NULL, @DURACIONMS bigint = NULL AS
BEGIN
  INSERT INTO dbo.MOCK_REPORT_INTEGRACIONES (ID_INTEGRACIONES, TIPOSERVICIO, METODOHTTP, ENDPOINT, REQUEST, RESPONSE, STATUSCODE, SUCCESS, ERROR, FECHAINICIO, FECHAFIN, DURACIONMS)
  VALUES (@ID_INTEGRACIONES, @TIPOSERVICIO, @METODOHTTP, @ENDPOINT, @REQUEST, @RESPONSE, @STATUSCODE, @SUCCESS, @ERROR, @FECHAINICIO, @FECHAFIN, @DURACIONMS);
  SELECT CAST(1 AS bit);
END
GO

-- Fénix sin datos: GA2 inserta una notificación al perfil ASERA_T (CreditoRepository.InsertarNotificacionFuncionarioExterno).
IF OBJECT_ID('dbo.MOCK_NOTIFICACIONES') IS NULL
  CREATE TABLE dbo.MOCK_NOTIFICACIONES (
    ID int IDENTITY(1,1) PRIMARY KEY, PERFIL nvarchar(50) NULL, MENSAJE nvarchar(max) NULL, MODULO nvarchar(100) NULL,
    USUARIO_ENVIA nvarchar(100) NULL, REGISTRADO datetime2 NOT NULL DEFAULT SYSDATETIME());
GO
CREATE OR ALTER PROCEDURE dbo.InsertarNotificacionFuncionarioExterno
  @Perfil nvarchar(50) = NULL, @Mensaje nvarchar(max) = NULL, @Modulo nvarchar(100) = NULL, @UsuarioEnvia nvarchar(100) = NULL AS
BEGIN
  INSERT INTO dbo.MOCK_NOTIFICACIONES (PERFIL, MENSAJE, MODULO, USUARIO_ENVIA) VALUES (@Perfil, @Mensaje, @Modulo, @UsuarioEnvia);
  SELECT 'OK' AS Resp;
END
GO
