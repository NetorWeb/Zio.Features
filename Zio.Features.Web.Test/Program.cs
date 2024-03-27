using Autofac;
using Zio.Features.Core.Extensions;
using Zio.Features.Core.Test;
using Zio.Features.DI.Autofac.Hosting;
using Zio.Features.EntityFrameworkCore;
using Zio.Features.EntityFrameworkCore.Hosting;
using Zio.Features.EntityFrameworkCore.Mysql.Extensions;

namespace Zio.Features.Web.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args).Inject();

            builder.Host.AddAutofac();

            builder.Services.AddZioDbContext<TestDbContext>(options =>
            {
                options.UseMysql();
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
