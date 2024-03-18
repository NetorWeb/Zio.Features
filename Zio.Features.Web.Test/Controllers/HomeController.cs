﻿using Microsoft.AspNetCore.Mvc;
using Zio.Features.Core;
using Zio.Features.Service.Test.IServices;

namespace Zio.Features.Web.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController(
        ITestService testService,
        ITest2Service test2Service,
        ITest3Service test3Service,
        ISecondService secondService
    ) : ControllerBase
    {
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
                作用域声明
            };

            return Results.Ok(result);
        }
    }
}