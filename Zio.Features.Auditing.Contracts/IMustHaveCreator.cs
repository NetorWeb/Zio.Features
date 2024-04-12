namespace Zio.Features.Auditing.Contracts;

public interface IMustHaveCreator<TCreator> 
{
    TCreator Creator { get; }
}

public interface IMustHaveCreator
{
    Guid CreatorId { get; }
}