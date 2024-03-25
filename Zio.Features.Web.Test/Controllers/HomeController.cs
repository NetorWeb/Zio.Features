using Microsoft.AspNetCore.Mvc;
using Zio.Features.Core;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.DI.Autofac.Abstraction;
using Zio.Features.Service.Test.IServices;
using Zio.Features.Service.Test.Services;

namespace Zio.Features.Web.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController(
        ITestService testService,
        ITest2Service test2Service,
        ITest3Service test3Service,
        ISecondService secondService,
        INamedResolver namedResolver,
        INamedService namedService,
        IDelegateInvokeService delegateInvokeService
    ) : ControllerBase
    {
        [Autowired] public ITestService TestService { get; set; }
        [Autowired] public ISecondService SecondService { get; set; }
        [Autowired] public ITest2Service Test2Service { get; set; }
        [Autowired] public ITest3Service Test3Service { get; set; }

        [HttpGet]
        public IResult Index()
        {
            object 作用域声明 = null;
            Scoped.Create((factory, scope) =>
            {
                作用域声明 = new
                {
                    Single = scope.ServiceProvider.GetService<ITestService>().GetData(),
                    Second = scope.ServiceProvider.GetService<ISecondService>().GetSecondData(),
                    Scope = scope.ServiceProvider.GetService<ITest2Service>().GetData(),
                    Transient = scope.ServiceProvider.GetService<ITest3Service>().GetData()
                };
            });

            var result = new
            {
                构造函数注入 = new
                {
                    Single = testService.GetData(),
                    Second = secondService.GetSecondData(),
                    Scope = test2Service.GetData(),
                    Transient = test3Service.GetData()
                },
                作用域声明,
                属性注入 = new
                {
                    Single = TestService.GetData(),
                    Second = SecondService.GetSecondData(),
                    Scope = Test2Service.GetData(),
                    Transient = Test3Service.GetData()
                }
            };

            return Results.Ok(result);
        }

        [HttpGet]
        public IResult Named()
        {
            var data = namedResolver.Get<INamedService>("Named1").GetName();
            var data2 = namedResolver.Get<INamedService>("Named2").GetName();
            var date3 = namedService.GetName();
            return Results.Ok(new
            {
                data,
                data2,
                date3
            });
        }

        [HttpGet]
        public IResult DelegateInvoke()
        {
            return Results.Ok(delegateInvokeService.GetData());
        }
    }
}