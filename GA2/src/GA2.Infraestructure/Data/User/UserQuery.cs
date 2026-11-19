namespace GA2.Infraestructure.Data
{
    /// <summary>
    /// Query string users database
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>20/03/2021</date>
    public static class UserQuery
    {
        public const string QueryGetAllUser = @"SELECT US.*,TI.TID_CODIGO FROM USR_USERS US INNER JOIN TID_TIPO_IDENTIFICACION TI ON TI.TID_ID=US.TID_ID WHERE USR_INHABILITADO = 0 AND USR_ELIMINADO = 0;";

        public const string QueryGetByUser = @"SELECT * FROM USR_USERS WHERE USR_ID=@USR_ID";

        public const string QueryInsertUser = @"INSERT INTO USR_USERS (USR_ID,USR_NOMBRE,USR_SEGUNDONOMBRE,USR_APELLIDO,USR_SEGUNDOAPELLIDO,USR_USERNAME,USR_PASSWORD,USR_IDENTIFICACION,TID_ID,USR_EMAIL,USR_CREADOPOR,USR_FECHACREACION,USR_ESTADO,ACTIVE_DIRECTORY,USR_INHABILITADO)VALUES (@USR_ID,@USR_NOMBRE,@USR_SEGUNDONOMBRE,@USR_APELLIDO,@USR_SEGUNDOAPELLIDO,@USR_USERNAME,@USR_CREDENTIAL,@USR_IDENTIFICACION,@TID_ID,@USR_EMAIL,@USR_CREADOPOR,GETDATE(),@USR_ESTADO,@ACTIVE_DIRECTORY,0);" + QueryGetByUser;

        public const string QueryGetIdentiUser = @"SELECT * FROM USR_USERS WHERE TID_ID=@TID_ID AND USR_IDENTIFICACION = @USR_IDENTIFICACION AND USR_ELIMINADO = 0"; 

        public const string QueryUpdateUser = @"UPDATE USR_USERS SET 
                                                USR_NOMBRE= @USR_NOMBRE,USR_SEGUNDONOMBRE=@USR_SEGUNDONOMBRE,USR_APELLIDO =@USR_APELLIDO,USR_SEGUNDOAPELLIDO = @USR_SEGUNDOAPELLIDO,USR_USERNAME = @USR_USERNAME
                                                ,USR_IDENTIFICACION = @USR_IDENTIFICACION,TID_ID =@TID_ID,USR_EMAIL = @USR_EMAIL,USR_ESTADO =@USR_ESTADO,ACTIVE_DIRECTORY=@ACTIVE_DIRECTORY WHERE USR_ID=@USR_ID;

                                                DECLARE @LSS_ID INT = (SELECT TOP 1 LSS_ID FROM LSS_SESION_LOGIN WHERE USR_ID = @USR_ID ORDER BY LSS_FECHA_INICIO_LOGIN DESC)  
  
                                                UPDATE LSS_SESION_LOGIN  
                                                SET LSS_ACTIVA = 0
                                                WHERE LSS_ID = @LSS_ID " + QueryGetByUser;

        public const string QueryUpdateStateUser = @"UPDATE USERS SET STATE=@STATE WHERE ID=@ID;" + QueryGetByUser;

        public const string QueryUpdatePasswordUser = @"UPDATE USR_USERS SET USR_PASSWORD = @USR_CREDENTIAL WHERE USR_ID=@USR_ID;" + QueryGetByUser;

        public const string QueryDelete = "UPDATE USR_USERS SET USR_ELIMINADO = 1 WHERE USR_ID=@USR_ID";
    }
}
