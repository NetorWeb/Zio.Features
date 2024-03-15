using Zio.Features.Core.DependencyInjection;
using Zio.Features.Service.Test.IServices;

namespace Zio.Features.Service.Test.Services
{
    public class TestService : ITestService, ISecondService,ISingletonDependency
    {

        private object? _cacheData;

        public object GetData()
        {
            if (_cacheData is not null)
            {
                return _cacheData;
            }

            var rd = new Random();

            _cacheData = new
            {
                number = rd.NextDouble(),
                name = nameof(TestService)
            };

            return _cacheData;
        }

        public object GetSecondData()
        {
            return _cacheData;
        }
    }

    public class Test2Service: ITest2Service, IScopedDependency
    {
        private object? _cacheData;

        public object GetData()
        {
            if (_cacheData is not null)
            {
                return _cacheData;
            }

            var rd = new Random();

            _cacheData = new
            {
                number = rd.NextDouble(),
                name = nameof(Test2Service)
            };

            return _cacheData;
        }
    }

    public class Test3Service : ITest3Service, ITransientDependency
    {
        private object? _cacheData;

        public object GetData()
        {
            if (_cacheData is not null)
            {
                return _cacheData;
            }

            var rd = new Random();

            _cacheData = new
            {
                number = rd.NextDouble(),
                name = nameof(Test3Service)
            };

            return _cacheData;
        }
    }

    public class Test4Service : ITest3Service, ITransientDependency
    {
        private object? _cacheData;

        public object GetData()
        {
            if (_cacheData is not null)
            {
                return _cacheData;
            }

            var rd = new Random();

            _cacheData = new
            {
                number = rd.NextDouble(),
                name = nameof(Test4Service)
            };

            return _cacheData;
        }
    }
}
