using ClinicaMedica;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ConsultaRepository>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/consultas", async (string? busca, string? status, string? ordem, ConsultaRepository banco) =>
    Results.Ok(await banco.Listar(busca, status, ordem)));

app.MapPost("/api/consultas", async (Consulta consulta, ConsultaRepository banco) =>
{
    if (string.IsNullOrWhiteSpace(consulta.Paciente) || string.IsNullOrWhiteSpace(consulta.Medico))
        return Results.BadRequest(new { mensagem = "Informe o paciente e o médico." });
    if (consulta.DataHora < DateTime.Now)
        return Results.BadRequest(new { mensagem = "A consulta deve ter uma data futura." });
    if (consulta.Status is not ("Agendada" or "Concluída" or "Cancelada"))
        return Results.BadRequest(new { mensagem = "Selecione uma situação válida." });

    var id = await banco.Adicionar(consulta);
    return Results.Created($"/api/consultas/{id}", new { id });
});

app.MapDelete("/api/consultas/{id:int}", async (int id, ConsultaRepository banco) =>
    await banco.Remover(id) ? Results.NoContent() : Results.NotFound());

app.Run();
