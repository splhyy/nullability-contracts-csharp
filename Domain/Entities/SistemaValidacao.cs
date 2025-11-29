using System.Diagnostics.CodeAnalysis;
using Domain.Exceptions;

namespace Domain.Entities;

public class SistemaValidacao
{
    private readonly List<Pessoa> _pessoas = new();

    [NotNull]
    public IReadOnlyList<Pessoa> Pessoas => _pessoas.AsReadOnly();

    // Método com contrato complexo de nulidade
    public bool TryAdicionarPessoa(
        [NotNullWhen(true)] Pessoa? pessoa,
        [MaybeNullWhen(true)] out string? mensagemSucesso,
        [NotNullWhen(false)] out string? erro)
    {
        mensagemSucesso = null;
        erro = null;

        if (pessoa is null)
        {
            erro = "Pessoa não pode ser nula";
            return false;
        }

        if (_pessoas.Any(p => p.Nome == pessoa.Nome))
        {
            erro = "Já existe uma pessoa com este nome";
            return false;
        }

        _pessoas.Add(pessoa);
        mensagemSucesso = $"Pessoa {pessoa.Nome} adicionada com sucesso";
        return true;
    }

    // Busca com contratos de retorno
    [return: MaybeNull]
    public Pessoa? BuscarPessoaPorEmail([AllowNull] string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return _pessoas.FirstOrDefault(p => 
            p.ObterEmailFormatado() == email.ToLowerInvariant());
    }

    [return: NotNullIfNotNull("defaultValue")]
    public string? BuscarNomePorEmail(
        [AllowNull] string? email, 
        [AllowNull] string? defaultValue = "Não encontrado")
    {
        var pessoa = BuscarPessoaPorEmail(email);
        return pessoa?.Nome ?? defaultValue;
    }

    // Validação com estado garantido
    [MemberNotNull(nameof(_pessoas))]
    public void ValidarEstadoSistema()
    {
        if (_pessoas is null)
            throw new InvalidEntityStateException("Lista de pessoas não pode ser nula");

        foreach (var pessoa in _pessoas)
        {
            if (string.IsNullOrWhiteSpace(pessoa.Nome))
                throw new InvalidEntityStateException("Todas as pessoas devem ter nome válido");
        }
    }
}