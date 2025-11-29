using Domain.Entities;

namespace Domain.Tests.Entities;

public class EnderecoTests
{
    [Fact]
    public void TryCriar_ComDadosValidos_DeveRetornarTrueECriarEndereco()
    {
        // Arrange
        var logradouro = "Av. Paulista";
        var cidade = "São Paulo";

        // Act
        var resultado = Endereco.TryCriar(logradouro, cidade, out var endereco, out var erro);

        // Assert
        Assert.True(resultado);
        Assert.NotNull(endereco);
        Assert.Null(erro);
        Assert.Equal(logradouro, endereco.Logradouro);
        Assert.Equal(cidade, endereco.Cidade);
    }

    [Theory]
    [InlineData(null, "São Paulo", "Logradouro é obrigatório")]
    [InlineData("Av. Paulista", null, "Cidade é obrigatória")]
    [InlineData("", "São Paulo", "Logradouro é obrigatório")]
    [InlineData("Av. Paulista", "", "Cidade é obrigatória")]
    public void TryCriar_ComDadosInvalidos_DeveRetornarFalseEErro(
        string? logradouro, string? cidade, string erroEsperado)
    {
        // Act
        var resultado = Endereco.TryCriar(logradouro, cidade, out var endereco, out var erro);

        // Assert
        Assert.False(resultado);
        Assert.Null(endereco);
        Assert.Equal(erroEsperado, erro);
    }

    [Fact]
    public void EhValido_ComEnderecoCompleto_DeveRetornarTrue()
    {
        // Arrange
        var endereco = new Endereco("Rua A", "Rio de Janeiro", "Complemento");

        // Act
        var resultado = endereco.EhValido();

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void EhValido_ComLogradouroVazio_DeveRetornarFalse()
    {
        // Arrange
        var endereco = new Endereco("  ", "Rio de Janeiro");

        // Act
        var resultado = endereco.EhValido();

        // Assert
        Assert.False(resultado);
    }
}