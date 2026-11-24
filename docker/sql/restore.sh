#!/usr/bin/env bash
# Restaura los respaldos de /backups en el SQL Server "sqlserver" si las bases aún no existen.
# Uso dentro del contenedor db-restore (imagen mssql, que trae sqlcmd).
set -euo pipefail

SQLCMD=(/opt/mssql-tools18/bin/sqlcmd -C -S sqlserver -U sa -P "$MSSQL_SA_PASSWORD" -b)

echo "Esperando a SQL Server..."
for i in $(seq 1 60); do
  "${SQLCMD[@]}" -Q "SELECT 1" >/dev/null 2>&1 && break
  sleep 5
done

restore() {
  local db="$1" bak="$2"
  if [ "$("${SQLCMD[@]}" -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = '$db'")" = "1" ]; then
    echo "La base $db ya existe; no se restaura."
    return
  fi
  echo "Restaurando $db desde $bak..."
  "${SQLCMD[@]}" -Q "
    SET NOCOUNT ON;
    DECLARE @files TABLE (LogicalName nvarchar(128), PhysicalName nvarchar(260), Type char(1), FileGroupName nvarchar(128),
      Size numeric(20,0), MaxSize numeric(20,0), FileId bigint, CreateLSN numeric(25,0), DropLSN numeric(25,0),
      UniqueId uniqueidentifier, ReadOnlyLSN numeric(25,0), ReadWriteLSN numeric(25,0), BackupSizeInBytes bigint,
      SourceBlockSize int, FileGroupId int, LogGroupGUID uniqueidentifier, DifferentialBaseLSN numeric(25,0),
      DifferentialBaseGUID uniqueidentifier, IsReadOnly bit, IsPresent bit, TDEThumbprint varbinary(32), SnapshotUrl nvarchar(360));
    INSERT INTO @files EXEC('RESTORE FILELISTONLY FROM DISK = ''/backups/$bak''');
    DECLARE @sql nvarchar(max) = N'RESTORE DATABASE [$db] FROM DISK = ''/backups/$bak'' WITH RECOVERY, REPLACE, STATS = 25';
    SELECT @sql += N', MOVE ''' + LogicalName + N''' TO ''/var/opt/mssql/data/$db' + '_' + CAST(FileId AS nvarchar(10))
      + CASE Type WHEN 'L' THEN N'.ldf' ELSE N'.mdf' END + N'''' FROM @files;
    EXEC(@sql);"
  echo "Base $db restaurada."
}

restore "$GA2_DB" "$GA2_BAK"
restore "$BUA_DB" "$BUA_BAK"
echo "Restauración terminada."

# Parametrización que el backup no trae (interruptores Dodo/TusDatos, catálogo de
# estados del cliente). Idempotente: se ejecuta en cada arranque, también cuando las
# bases ya existían, para que los clones viejos también queden al día.
if [ -f /seed.sql ]; then
  echo "Aplicando seed de parametrización en $GA2_DB..."
  "${SQLCMD[@]}" -d "$GA2_DB" -i /seed.sql
  echo "Seed aplicado."
fi
