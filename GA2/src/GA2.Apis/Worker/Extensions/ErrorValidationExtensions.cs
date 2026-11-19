using GA2.Apis.Identity.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;

namespace Worker.Extensions
{
    /// <summary>
    /// ErrorValidation Extensions
    /// </summary>
    public static class ErrorValidationExtensions
    {
        internal static IApplicationBuilder UseErrorValidation(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<ErrorValidationMiddleware>();
            return app;
        }
    }
}
