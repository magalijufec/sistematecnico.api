using System.Security.Claims;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IErrorLogRepository errorLogRepository)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                try
                {
                    var usuario =
                        context.User?
                            .FindFirst(ClaimTypes.NameIdentifier)?
                            .Value;

                    var error = new ErrorLog
                    {
                        Fecha = DateTime.Now,

                        Mensaje = ex.Message,

                        StackTrace = ex.StackTrace,

                        InnerException =
                            ex.InnerException?.ToString(),

                        Endpoint =
                            context.Request.Path,

                        Metodo =
                            context.Request.Method,

                        Usuario =
                            usuario,

                        Ip =
                            context.Connection.RemoteIpAddress?
                                .ToString()
                    };

                    await errorLogRepository.RegistrarAsync(error);
                }
                catch
                {
                    // No hacemos nada si falla el registro
                    // del propio error.
                }

                throw;
            }
        }
    }
}
