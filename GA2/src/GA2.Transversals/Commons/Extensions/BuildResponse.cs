namespace GA2.Transversals.Commons.Extensions
{
    public static class BuildResponse
    {
        public static Response<T> BuildSuccess<T>(this T data, string message = "") where T : class
        {
            return Build(data, message, true);
        }

        public static Response<T> BuildError<T>(this string message, T data = null) where T : class
        {
            return Build(data, message, false);
        }

        public static Response<T> Build<T>(this T data, string message, bool isSuccess) where T : class
        {
            return new()
            {
                Data = data,
                IsSuccess = isSuccess,
                ReturnMessage = message
            };
        }
    }
}
