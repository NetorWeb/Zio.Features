using System.ComponentModel.DataAnnotations.Schema;

namespace Zio.Features.Core.Test.Entity;

[Table("author")]
public class Author
{
    public int Id { get; set; }

    public string Name { get; set; }
}