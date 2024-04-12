namespace Zio.Features.Auditing.Contracts;

public interface IMayHaveCreator<TCreator>
{
    TCreator? Creator { get; }
}

public interface IMayHaveCreator
{
    Guid? CreatorId { get; }
}