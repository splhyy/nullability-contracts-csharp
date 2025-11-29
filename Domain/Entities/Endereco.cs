using System.Diagnostics.CodeAnalysis;
using Domain.Exceptions;

namespace Domain.Entities;

public class Endereco
{
    private string _logradouro = string.Empty;
    private string _cidade = string.Empty;

    public Guid Id { get; private set; }

    [DisallowNull]
    public string Logradouro 
    { 
        get => _logradouro;
        set => _logradouro = value ?? throw new NullabilityContractException("Logradouro não pode ser nulo");
    }

    [DisallowNull]
    public string Cidade 
    { 
        get => _cidade;
        set => _cidade = value ?? throw new NullabilityContractException("Cidade não pode ser nula");
    }

    [AllowNull]
    public string? Complemento { get; set; }

    public Endereco(string logradouro, string cidade, string? complemento = null)
    {
        Logradouro = logradouro ?? throw new ArgumentNullException(nameof(logradouro));
        Cidade = cidade ?? throw new ArgumentNullException(nameof(cidade));
        Complemento = complemento;
        Id = Guid.NewGuid();
    }

    public bool EhValido()
    {
        return !string.IsNullOrWhiteSpace(Logradouro) && 
               !string.IsNullOrWhiteSpace(Cidade);
    }

    // Try-pattern para criação
    public static bool TryCriar(
        [NotNullWhen(true)] string? logradouro,
        [NotNullWhen(true)] string? cidade,
        [MaybeNullWhen(false)] out Endereco endereco,
        [NotNullWhen(false)] out string? erro)
    {
        endereco = null;
        erro = null;

        if (string.IsNullOrWhiteSpace(logradouro))
        {
            erro = "Logradouro é obrigatório";
            return false;
        }

        if (string.IsNullOrWhiteSpace(cidade))
        {
            erro = "Cidade é obrigatória";
            return false;
        }

        endereco = new Endereco(logradouro, cidade);
        return true;
    }
}