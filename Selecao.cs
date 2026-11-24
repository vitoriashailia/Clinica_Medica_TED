namespace ClinicaMedica;

// Seleção (filtragem) implementada manualmente sobre uma List<Consulta>.
// Não usa LINQ, Where nem a cláusula WHERE do SQLite.
public static class Selecao
{
    // Parâmetro 1: status         -> "Agendada", "Concluída", "Cancelada" ou "Todas".
    // Parâmetro 2: especialidade  -> nome da especialidade (vazio = qualquer uma).
    // Um elemento só é selecionado se satisfizer os DOIS parâmetros informados.
    public static List<Consulta> Selecionar(List<Consulta> lista, string? status, string? especialidade)
    {
        bool filtraStatus = !string.IsNullOrWhiteSpace(status) && status != "Todas";
        bool filtraEspecialidade = !string.IsNullOrWhiteSpace(especialidade);
        var alvoEspecialidade = especialidade?.Trim() ?? "";

        var selecionadas = new List<Consulta>();

        for (int i = 0; i < lista.Count; i++)
        {
            var consulta = lista[i];

            if (filtraStatus && consulta.Status != status)
                continue;

            if (filtraEspecialidade &&
                !string.Equals(consulta.Especialidade.Trim(), alvoEspecialidade, StringComparison.OrdinalIgnoreCase))
                continue;

            selecionadas.Add(consulta);
        }

        return selecionadas;
    }
}
