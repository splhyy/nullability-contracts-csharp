using System.Diagnostics.CodeAnalysis;
using Domain.Exceptions;

namespace Domain.Entities;

public class Pessoa
{
    private string _nome = string.Empty;
    private string? _email;
    private readonly List<Endereco> _enderecos = new();

    public Guid Id { get; private set; }
    public int Idade { get; private set; }

    [DisallowNull]
    public string Nome 
    { 
        get => _nome;
        set => _nome = value ?? throw new NullabilityContractException("Nome não pode ser nulo");
    }

    [AllowNull]
    public string? Email 
    { 
        get => _email;
        set => _email = value;
    }

    [NotNull]
    public IReadOnlyList<Endereco> Enderecos => _enderecos.AsReadOnly();

    public Pessoa(string nome, int idade, string? email = null)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Idade = idade >= 0 ? idade : throw new InvalidEntityStateException("Idade não pode ser negativa");
        Email = email;
        Id = Guid.NewGuid();
    }

    // Try-pattern para adicionar endereço
    public bool TryAdicionarEndereco([NotNullWhen(true)] Endereco? endereco, [NotNullWhen(false)] out string? erro)
    {
        erro = null;
        
        if (endereco is null)
        {
            erro = "Endereço não pode ser nulo";
            return false;
        }

        if (!endereco.EhValido())
        {
            erro = "Endereço não é válido";
            return false;
        }

        _enderecos.Add(endereco);
        return true;
    }

    // Método com contrato de nulidade
    [return: NotNullIfNotNull("defaultValue")]
    public string? ObterEmailFormatado([AllowNull] string? defaultValue = null)
    {
        return string.IsNullOrEmpty(Email) ? defaultValue : Email.ToLowerInvariant();
    }

    [MemberNotNull(nameof(_nome))]
    private void ValidarEstado()
    {
        if (string.IsNullOrWhiteSpace(_nome))
            throw new InvalidEntityStateException("Pessoa deve ter um nome válido");
    }
}