using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Logging;
using Zio.Features.Core;
using Zio.Features.Core.Extensions;
using Zio.Features.Core.Logging;

namespace Zio.Features.Validation.Abstractions;

public class ZioValidationException: ZioException,IHasLogLevel,IHasValidationErrors,IExceptionWithSelfLogging
{
    public LogLevel LogLevel { get; set; }
    public IList<ValidationResult> ValidationErrors { get; }

    public ZioValidationException()
    {
        LogLevel = LogLevel.Warning;
        ValidationErrors = new List<ValidationResult>();
    }
    
    public ZioValidationException(string message)
    :base(message)
    {
        LogLevel = LogLevel.Warning;
        ValidationErrors = new List<ValidationResult>();
    }

    public ZioValidationException(IList<ValidationResult> validationErrors)
    {
        ValidationErrors = validationErrors;
        LogLevel = LogLevel.Warning;
    }

    public ZioValidationException(string message, IList<ValidationResult> validationErrors)
    :base(message)
    {
        validationErrors = validationErrors;
        LogLevel = LogLevel.Warning;
    }
    
    public ZioValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }
    
    public void Log(ILogger logger)
    {
        if (ValidationErrors.IsNullOrEmpty())
        {
            return;
        }
        
        var validationErrors = new StringBuilder();
        validationErrors.AppendLine("There are " + ValidationErrors.Count + " validation errors:");
        foreach (var validationResult in ValidationErrors)
        {
            var memberNames = "";
            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
            }

            validationErrors.AppendLine(validationResult.ErrorMessage + memberNames);
        }

        logger.LogWithLevel(LogLevel, validationErrors.ToString());
    }
}