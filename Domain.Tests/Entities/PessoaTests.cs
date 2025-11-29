using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities;

public class PessoaTests
{
    [Fact]
    public void CriarPessoa_ComNomeNulo_DeveLancarExcecao()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Pessoa(null!, 25));
    }

    [Fact]
    public void CriarPessoa_ComDadosValidos_DeveCriarInstancia()
    {
        // Arrange
        var nome = "João Silva";
        var email = "joao@email.com";

        // Act
        var pessoa = new Pessoa(nome, 25, email);

        // Assert
        Assert.NotNull(pessoa);
        Assert.Equal(nome, pessoa.Nome);
        Assert.Equal(email, pessoa.Email);
        Assert.Empty(pessoa.Enderecos);
    }

    [Fact]
    public void TryAdicionarEndereco_ComEnderecoValido_DeveRetornarTrue()
    {
        // Arrange
        var pessoa = new Pessoa("Maria", 30);
        var endereco = new Endereco("Rua A", "São Paulo");

        // Act
        var resultado = pessoa.TryAdicionarEndereco(endereco, out var erro);

        // Assert
        Assert.True(resultado);
        Assert.Null(erro);
        Assert.Single(pessoa.Enderecos);
    }

    [Fact]
    public void TryAdicionarEndereco_ComEnderecoNulo_DeveRetornarFalse()
    {
        // Arrange
        var pessoa = new Pessoa("Carlos", 35);

        // Act
        var resultado = pessoa.TryAdicionarEndereco(null, out var erro);

        // Assert
        Assert.False(resultado);
        Assert.NotNull(erro);
        Assert.Empty(pessoa.Enderecos);
    }

    [Theory]
    [InlineData(null, "padrão", "padrão")]
    [InlineData("teste@email.com", "padrão", "teste@email.com")]
    [InlineData("", "padrão", "padrão")]
    public void ObterEmailFormatado_ComDefaultValue_DeveRetornarValorCorreto(
        string? email, string? defaultValue, string? expected)
    {
        // Arrange
        var pessoa = new Pessoa("Teste", 40, email);

        // Act
        var resultado = pessoa.ObterEmailFormatado(defaultValue);

        // Assert
        Assert.Equal(expected, resultado);
    }

    [Fact]
    public void SetNome_ComValorNulo_DeveLancarExcecao()
    {
        // Arrange
        var pessoa = new Pessoa("Inicial", 25);

        // Act & Assert
        Assert.Throws<NullabilityContractException>(() => pessoa.Nome = null!);
    }
}