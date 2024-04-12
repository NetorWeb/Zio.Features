namespace Zio.Features.EntityFrameworkCore;

public interface IZioEfCoreDbContext:IEfCoreDbContext
{
    void Initialize(ZioEfCoreDbContextInitializationContext initializationContext);
}