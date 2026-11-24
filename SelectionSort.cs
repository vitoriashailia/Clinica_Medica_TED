using System.Globalization;

namespace ClinicaMedica;

// Selection Sort implementado manualmente, sem OrderBy, List.Sort ou ORDER BY do SQLite.
public static class SelectionSort
{
    // Ordena a própria lista (in-place) segundo o critério recebido.
    // A cada volta do laço externo, procura o menor elemento da parte ainda
    // não ordenada (posições i..fim) e o troca com o elemento da posição i.
    public static void Ordenar(List<Consulta> lista, Comparison<Consulta> comparar)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            int posicaoMenor = i;
            for (int j = i + 1; j < lista.Count; j++)
            {
                if (comparar(lista[j], lista[posicaoMenor]) < 0)
                    posicaoMenor = j;
            }

            if (posicaoMenor != i)
                (lista[i], lista[posicaoMenor]) = (lista[posicaoMenor], lista[i]);
        }
    }

    // Critério 1: nome do paciente em ordem alfabética (desempate: data/hora e Id).
    public static int PorPaciente(Consulta a, Consulta b)
    {
        int resultado = string.Compare(a.Paciente, b.Paciente, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
        if (resultado != 0) return resultado;
        resultado = a.DataHora.CompareTo(b.DataHora);
        return resultado != 0 ? resultado : a.Id.CompareTo(b.Id);
    }

    // Critério 2: data e horário, da mais próxima para a mais distante (desempate: paciente e Id).
    public static int PorData(Consulta a, Consulta b)
    {
        int resultado = a.DataHora.CompareTo(b.DataHora);
        if (resultado != 0) return resultado;
        resultado = string.Compare(a.Paciente, b.Paciente, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
        return resultado != 0 ? resultado : a.Id.CompareTo(b.Id);
    }
}
