using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities;

public class SistemaValidacaoTests
{
    [Fact]
    public void TryAdicionarPessoa_ComPessoaValida_DeveRetornarTrueEMensagem()
    {
        // Arrange
        var sistema = new SistemaValidacao();
        var pessoa = new Pessoa("João", 25);

        // Act
        var resultado = sistema.TryAdicionarPessoa(pessoa, out var mensagemSucesso, out var erro);

        // Assert
        Assert.True(resultado);
        Assert.NotNull(mensagemSucesso);
        Assert.Null(erro);
        Assert.Single(sistema.Pessoas);
    }

    [Fact]
    public void TryAdicionarPessoa_ComPessoaNula_DeveRetornarFalseEErro()
    {
        // Arrange
        var sistema = new SistemaValidacao();

        // Act
        var resultado = sistema.TryAdicionarPessoa(null, out var mensagemSucesso, out var erro);

        // Assert
        Assert.False(resultado);
        Assert.Null(mensagemSucesso);
        Assert.NotNull(erro);
        Assert.Empty(sistema.Pessoas);
    }

    [Fact]
    public void BuscarPessoaPorEmail_ComEmailExistente_DeveRetornarPessoa()
    {
        // Arrange
        var sistema = new SistemaValidacao();
        var pessoa = new Pessoa("Maria", 30, "maria@email.com");
        sistema.TryAdicionarPessoa(pessoa, out _, out _);

        // Act
        var resultado = sistema.BuscarPessoaPorEmail("maria@email.com");

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Maria", resultado.Nome);
    }

    [Fact]
    public void BuscarPessoaPorEmail_ComEmailInexistente_DeveRetornarNull()
    {
        // Arrange
        var sistema = new SistemaValidacao();

        // Act
        var resultado = sistema.BuscarPessoaPorEmail("inexistente@email.com");

        // Assert
        Assert.Null(resultado);
    }

    [Theory]
    [InlineData(null, "padrão", "padrão")]
    [InlineData("teste@email.com", "padrão", "Teste")]
    public void BuscarNomePorEmail_ComVariados_DeveRetornarValorEsperado(
        string? email, string? defaultValue, string expected)
    {
        // Arrange
        var sistema = new SistemaValidacao();
        sistema.TryAdicionarPessoa(new Pessoa("Teste", 25, "teste@email.com"), out _, out _);

        // Act
        var resultado = sistema.BuscarNomePorEmail(email, defaultValue);

        // Assert
        Assert.Equal(expected, resultado);
    }

    [Fact]
    public void ValidarEstadoSistema_ComPessoasValidas_NaoDeveLancarExcecao()
    {
        // Arrange
        var sistema = new SistemaValidacao();
        sistema.TryAdicionarPessoa(new Pessoa("Válido", 25), out _, out _);

        // Act & Assert
        var exception = Record.Exception(() => sistema.ValidarEstadoSistema());
        Assert.Null(exception);
    }
}