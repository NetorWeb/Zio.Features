using System.ComponentModel.DataAnnotations;

namespace Zio.Features.Validation.Abstractions;

public interface IHasValidationErrors
{
    IList<ValidationResult> ValidationErrors { get; }
}