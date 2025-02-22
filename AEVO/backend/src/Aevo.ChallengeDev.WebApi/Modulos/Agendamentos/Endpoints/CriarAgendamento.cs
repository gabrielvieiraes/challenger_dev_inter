using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Models;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Endpoints;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;
using Aevo.ChallengeDev.WebApi.Utils;
using Aevo.CommonLib.Results;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Endpoints;

public record CriarAgendamento
{
    public required string Timezone { get; set; }
    public required Guid SalaId { get; init; }
    public required Guid UsuarioId { get; set; }
    public required DateTime Inicio { get; init; }
    public required DateTime Fim { get; init; }
}

public record CriarAgendamentoResponse
{
    public required Guid AgendamentoId { get; init; }
}

public class CriarAgendamentoHandler(Context context) : ICaseHandler<CriarAgendamento, CriarAgendamentoResponse>
{
    public async Task<Result<CriarAgendamentoResponse>> Handle(CriarAgendamento req, CancellationToken ct)
    {
        try
        {
            var result = ValidateInput(req);

            if (!result.IsSuccess)
            {
                return Result.Invalid(new AppError() { ErrorMessage = result.AsError().Errors?.ToString() });
            }

            var agendamento = new Agendamento()
            {
                Id = Guid.NewGuid(),
                Inicio = TimezoneUtil.ConvertDateToUtc(req.Inicio, req.Timezone),
                Fim = TimezoneUtil.ConvertDateToUtc(req.Fim, req.Timezone),
                SalaId = req.SalaId,
                UsuarioId = req.UsuarioId,
                CreationTime = DateTime.UtcNow,
                Timezone = req.Timezone,
            };

            context.Agendamentos.Add(agendamento);

            await context.SaveChangesAsync(ct);

            return new CriarAgendamentoResponse
            {
                AgendamentoId = agendamento.Id,
            };
        }
        catch (Exception ex)
        {
            return Result.Error("Não foi possível criar o agendamento, tente novamente mais tarde.");
        }
    }

    private Result ValidateInput(CriarAgendamento req)
    {
        if (req == null)
        {
            return Result.Invalid();
        }

        if (String.IsNullOrEmpty(req.Timezone))
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi informado o fuso horario." });
        }

        DateTime now = DateTime.UtcNow;
        DateTime maxDataFutura = DateTime.UtcNow.AddYears(1);

        if (req.Inicio >= req.Fim)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "O horário de início não pode ser maior ou igual ao horário de término." });
        }

        if (req.Inicio.Date < now.Date || req.Fim.Date < now.Date)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "O agendamento não pode ser feito no passado." });
        }

        if (req.Inicio.Date > maxDataFutura.Date || req.Fim.Date > maxDataFutura.Date)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "A data de início ou fim está muito no futuro." });
        }

        var existSchedulingWithSameData = context.Agendamentos.Any(x => x.SalaId == req.SalaId
                    && x.Inicio < TimezoneUtil.ConvertDateToUtc(req.Fim, req.Timezone)
                    && x.Fim > TimezoneUtil.ConvertDateToUtc(req.Inicio, req.Timezone));

        if (existSchedulingWithSameData)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Já existe um agendamento cadastrado." });
        }

        return Result.Ok();
    }
}