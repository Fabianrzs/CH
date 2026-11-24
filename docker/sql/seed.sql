-- Seed idempotente de parametrización para el entorno de pruebas (GA2_DEVELOP).
-- Lo ejecuta docker/sql/restore.sh después de restaurar, en cada arranque.
-- Rellena datos que el backup no trae y que hacen falta para que las integraciones
-- contra MockIntegrations funcionen. Todo es parametrización en BD; no toca código de GA2.
SET NOCOUNT ON;

-------------------------------------------------------------------------------
-- 1) Interruptores de integración (PIN_PARAMETROS_INTEGRACIONES).
--    El backup trae AQM/Bua/Fenix/IDVision/Vigia pero NO Dodo ni TusDatos.
--    * Sin Dodo  -> api/solicitud/ObtenerFlujoDodo revienta con NullReference
--      (GestorDocumentSolicitudes: .Where(DODO).FirstOrDefault().VALOR sobre null).
--    * Sin TusDatos -> api/terceros/validacion/actualizacion/vigia devuelve
--      "La integración [TusDatos] se encuentra inactiva o no configurada".
--    VALOR debe ser exactamente 'true' (el código compara == "true").
-------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.PIN_PARAMETROS_INTEGRACIONES WHERE UPPER(INTEGRACION) = 'DODO')
    INSERT INTO dbo.PIN_PARAMETROS_INTEGRACIONES (PARAM_ID, INTEGRACION, PARAMETRO, VALOR)
    VALUES (NEWID(), 'Dodo', 'Estado', 'true');

IF NOT EXISTS (SELECT 1 FROM dbo.PIN_PARAMETROS_INTEGRACIONES WHERE UPPER(INTEGRACION) = 'TUSDATOS')
    INSERT INTO dbo.PIN_PARAMETROS_INTEGRACIONES (PARAM_ID, INTEGRACION, PARAMETRO, VALOR)
    VALUES (NEWID(), 'TusDatos', 'Estado', 'true');

-------------------------------------------------------------------------------
-- 2) Catálogo de estados/causales del cliente.
--    ECL_ESTADO_CLIENTE y CEC_CAUSALES_ESTADO_CLIENTE vienen VACÍAS en el backup,
--    así que ObtenerEstadosCliente / ObtenerCausalEstadoCliente devuelven [].
--    Ambas PK son IDENTITY: hay que usar SET IDENTITY_INSERT (sin esto el insert
--    con ID explícito falla con "Msg 544 ... IDENTITY_INSERT is set to OFF").
--    1 = VIGENTE es el estado que GA2 trata como cliente activo.
-------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.ECL_ESTADO_CLIENTE)
BEGIN
    SET IDENTITY_INSERT dbo.ECL_ESTADO_CLIENTE ON;
    INSERT INTO dbo.ECL_ESTADO_CLIENTE (ECL_ID, CEC_ID, CEL_OBSERVACIONES)
    VALUES (1, 1, 'VIGENTE'),
           (2, 2, 'BLOQUEADO'),
           (3, 3, 'ANULADO');
    SET IDENTITY_INSERT dbo.ECL_ESTADO_CLIENTE OFF;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.CEC_CAUSALES_ESTADO_CLIENTE)
BEGIN
    SET IDENTITY_INSERT dbo.CEC_CAUSALES_ESTADO_CLIENTE ON;
    INSERT INTO dbo.CEC_CAUSALES_ESTADO_CLIENTE (CEC_ID, ECL_ID, CEC_CAUSAL, CEC_ACTIVO)
    VALUES (1, 1, 'SIN CAUSAL', 1),
           (2, 2, 'BLOQUEO ADMINISTRATIVO', 1),
           (3, 3, 'ANULACION', 1);
    SET IDENTITY_INSERT dbo.CEC_CAUSALES_ESTADO_CLIENTE OFF;
END;
