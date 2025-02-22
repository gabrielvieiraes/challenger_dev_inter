using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Utils;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Endpoints;

public record ExcluirAgendamento()
{
    public required Guid AgendamentoId { get; set; }
    public required Guid UsuarioId { get; set; }
};

public class ExcluiAgendamentoHandler(Context context) : ICaseHandler<ExcluirAgendamento, Unit>
{
    public async Task<Result<Unit>> Handle(ExcluirAgendamento req, CancellationToken ct)
    {
        try
        {
            var result = ValidateInput(req);

            if (!result.IsSuccess)
            {
                return Result.Invalid(new AppError() { ErrorMessage = result.AsError().Errors?.ToString() });
            }

            await context.Agendamentos.Where(x => x.Id == req.AgendamentoId).ExecuteDeleteAsync();

            return Result.Ok(Unit.Value);
        }
        catch (Exception ex)
        {
            throw new Exception("Não foi possível excluir o agendamento, tente novamente mais tarde.");
        }
    }

    private Result ValidateInput(ExcluirAgendamento req)
    {
        if (req == null)
        {
            return Result.Invalid();
        }

        var isDifferentUserOwner = context.Agendamentos.Any(x => x.Id == req.AgendamentoId && x.UsuarioId != req.UsuarioId);

        if (isDifferentUserOwner)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi possivel efetuar a exclusão para esse agendamento, tente novamente mais tarde." });
        }

        return Result.Ok();
    }
}