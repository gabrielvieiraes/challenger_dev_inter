using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Models;
using Aevo.ChallengeDev.WebApi.Utils;
using Aevo.CommonLib.Results;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Endpoints;

public record EditarAgendamento
{
    public required Guid AgendamentoId { get; init; }
    public required string Timezone { get; set; }
    public required Guid SalaId { get; set; }
    public required Guid UsuarioId { get; set; }
    public required DateTime Inicio { get; set; }
    public required DateTime Fim { get; set; }
}

public class EditarAgendamentoHandler(Context context) : ICaseHandler<EditarAgendamento, Unit>
{
    public async Task<Result<Unit>> Handle(EditarAgendamento req, CancellationToken ct)
    {
        try
        {
            var result = ValidateInput(req);

            if (!result.IsSuccess)
            {
                return Result.Invalid(new AppError() { ErrorMessage = result.AsError().Errors?.ToString() });
            }

            var agendamento = context.Agendamentos.FirstOrDefault(x => x.Id == req.AgendamentoId && x.UsuarioId == req.UsuarioId);

            if (agendamento != null)
            {
                agendamento.Inicio = TimezoneUtil.ConvertDateToUtc(req.Inicio, req.Timezone);
                agendamento.Fim = TimezoneUtil.ConvertDateToUtc(req.Fim, req.Timezone);
                agendamento.SalaId = req.SalaId;
                agendamento.UsuarioId = req.UsuarioId;
                agendamento.Timezone = req.Timezone;

                context.Agendamentos.Update(agendamento);

                await context.SaveChangesAsync(ct);

                return Result.Ok(Unit.Value);
            }
            else
            {
                return Result.Forbidden();
            }
        }
        catch (Exception ex)
        {
            return Result.Error("Não foi possível editar o agendamento, tente novamente mais tarde.");
        }
    }

    private Result ValidateInput(EditarAgendamento req)
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

        var existSchedulingWithSameData = context.Agendamentos.Any(x => x.Id != req.AgendamentoId
                    && x.SalaId == req.SalaId
                    && x.Inicio < TimezoneUtil.ConvertDateToUtc(req.Fim, req.Timezone)
                    && x.Fim > TimezoneUtil.ConvertDateToUtc(req.Inicio, req.Timezone));

        if (existSchedulingWithSameData)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Já existe um agendamento cadastrado." });
        }

        return Result.Ok();
    }
}