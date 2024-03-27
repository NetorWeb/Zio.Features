using Zio.Features.Core.DependencyInjection;
using Zio.Features.Core.Test;
using Zio.Features.Core.Test.Entity;
using Zio.Features.EntityFrameworkCore;
using Zio.Features.Service.Test.IServices;

namespace Zio.Features.Service.Test.Services;

public class EfCoreService: IEfCoreService, ITransientDependency
{
    private readonly ZioDbContext<TestDbContext> _context;

    public EfCoreService(
        ZioDbContext<TestDbContext> context
        )
    {
        _context = context;
    }

    public object GetData()
    {
        return _context.Set<Author>().AsQueryable().ToList();
    }
}