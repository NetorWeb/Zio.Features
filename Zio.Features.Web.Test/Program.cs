using Zio.Features.DI.Autofac.Hosting;

namespace Zio.Features.Web.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.AddAutofac();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
