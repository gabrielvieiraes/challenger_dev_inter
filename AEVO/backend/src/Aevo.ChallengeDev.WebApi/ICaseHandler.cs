using Aevo.CommonLib.Results;

namespace Aevo.ChallengeDev.WebApi;

public interface ICaseHandler<in TRequest, TResponse>
{
    Task<Result<TResponse>> Handle(TRequest req, CancellationToken ct);
}