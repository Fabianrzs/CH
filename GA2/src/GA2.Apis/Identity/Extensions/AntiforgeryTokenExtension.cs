using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace GA2.Apis.Identity.Extensions
{
    /// <summary>
    /// Provider CSRF token prevention
    /// </summary>
    public static class AntiforgeryTokenExtension
    {
        internal static IApplicationBuilder UseAntiforgeryToken(this IApplicationBuilder app, IAntiforgery antiforgery)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.Use(next => context =>
            {
                string path = context.Request.Path.Value;
                string urlAreas = "/api/obtenertoken";
                if (path.ToLowerInvariant().Equals(urlAreas))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    var cookieOptions = new CookieOptions
                    {
                        // Set the secure flag, which Chrome's changes will require for SameSite none.
                        // Note this will also require you to be running on HTTPS.
                        Secure = true,

                        // Set the cookie to HTTP only which is good practice unless you really do need
                        // to access it client side in scripts.
                        HttpOnly = true,
                        // Add the SameSite attribute, this will emit the attribute with a value of none.
                        // sameSiteMode.Strict = Indica que el cliente solo debe enviar la cookie con solicitudes del "mismo sitio".
                        SameSite = SameSiteMode.Strict,
                        // The client should follow its default cookie policy.
                        // SameSite = SameSiteMode.Unspecified
                        IsEssential = true
                    };
                    context.Response.Cookies.Append("XSRFTOKEN", tokens.RequestToken, cookieOptions);
                }
                return next(context);
            });
            return app;
        }
    }
}
