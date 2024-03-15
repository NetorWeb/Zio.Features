using Microsoft.AspNetCore.Mvc;
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
            return Results.Ok(new
            {
               Single = testService.GetData(),
               Second = secondService.GetSecondData(),
               Scope = test2Service.GetData(),
               Transient = test3Service.GetData()
            });
        }
    }
}
