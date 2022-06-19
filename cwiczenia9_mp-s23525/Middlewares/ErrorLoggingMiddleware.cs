using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                await ExceptionLogger(context, e);
            }
        }

        private async Task ExceptionLogger(HttpContext context, Exception e)
        {
            await using var stream = new StreamWriter("logs.txt", true); //logowanie wsztstkich bledow do logs.txt
            await stream.WriteLineAsync($"{DateTime.Now},{context.TraceIdentifier},{e.HResult}");
            await _next(context);
        }
    }
}
