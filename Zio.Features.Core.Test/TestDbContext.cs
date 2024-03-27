using Microsoft.EntityFrameworkCore;
using Zio.Features.Core.Test.Entity;
using Zio.Features.EntityFrameworkCore;

namespace Zio.Features.Core.Test;

public class TestDbContext:ZioDbContext<TestDbContext>
{
    protected TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Author { get; set; }

}