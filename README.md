# 🔒 Contratos de Nulidade em C# com Atributos

Sistema demonstrando o uso de **Nullable Reference Types** e atributos de análise para contratos explícitos de nulidade em C#, implementado como trabalho acadêmico de POO.

## 🎯 Objetivo

Implementar contratos de nulidade explícitos usando atributos de análise do C# para garantir segurança de tipos em tempo de compilação e runtime, seguindo os requisitos:
- Habilitar nullable reference types
- Usar atributos de análise para modelar associações
- Implementar try-patterns sem null-forgiving
- Garantir coleções não nulas
- Escrever testes que comprovem os contratos

## 🛠️ Tecnologias

- **.NET 9.0**
- **C# 12.0** com Nullable Reference Types
- **xUnit** para testes unitários
- **Atributos de Análise** do `System.Diagnostics.CodeAnalysis`

## 📋 Funcionalidades Implementadas

### ✅ Contratos de Nulidade

| Atributo | Onde Aplicado | Justificativa |
|----------|---------------|---------------|
| `[DisallowNull]` | `Pessoa.Nome`, `Endereco.Logradouro` | Propriedades obrigatórias que nunca podem ser nulas |
| `[AllowNull]` | `Pessoa.Email`, `Endereco.Complemento` | Propriedades opcionais que podem ser nulas |
| `[NotNullWhen(true)]` | `TryAdicionarEndereco(endereco)` | Parâmetro não nulo quando método retorna true |
| `[MaybeNullWhen(false)]` | `TryCriar(out endereco)` | Parâmetro de saída nulo quando método retorna false |
| `[MemberNotNull]` | `ValidarEstado()` | Garante que campos são inicializados após execução |
| `[NotNull]` | `Pessoa.Enderecos` | Coleção sempre retornada (pode ser vazia, mas não nula) |

### 🏗️ Padrões Implementados

- **Try-Pattern** com contratos de nulidade explícitos
- **Inicialização tardia** com validação de estado
- **Coleções não nulas** sempre inicializadas no construtor
- **Guard clauses** com exceções específicas do domínio
- **Encapsulamento** de invariantes com setters privados

## 🏗️ Arquitetura
````
NullabilityContracts/
├── 📁 Domain/
│ ├── 📁 Entities/ # Entidades com contratos
│ │ ├── Pessoa.cs # Modelo com try-patterns
│ │ ├── Endereco.cs # Validações com contratos
│ │ └── SistemaValidacao.cs # Serviço com associações
│ ├── 📁 Exceptions/ # Exceções específicas
│ │ └── DomainExceptions.cs
│ └── Domain.csproj # Nullable habilitado
└── 📁 Domain.Tests/
└── 📁 Entities/ # Testes dos contratos
````

## 🚀 Como Executar

# Clone o repositório
````
git clone https://github.com/splhyy/nullability-contracts-csharp.git
cd nullability-contracts-csharp
````
# Restaurar pacotes
````
dotnet restore
````
# Compilar (verificar warnings de nulidade)
````
dotnet build
````
# Executar testes
````
dotnet test
````
## 📊 Resultados de Compilação
##✅ Build sem Warnings Relevantes
Construir êxito(s) com X aviso(s) em X.Xs

Nota: Os avisos restantes são relacionados a configuração e não a violações de contrato de nulidade

## ✅ Testes Completos
````
Resumo do teste: total: 15; falhou: 0; bem-sucedido: 15; ignorado: 0
````
## 🧪 Testes dos Contratos
Os testes validam especificamente:

1. Ausência de Warnings Relevantes
Compilação sem warnings de nulidade significativos

Contratos respeitados em tempo de compilação

2. Caminhos True/False dos Try-Patterns
````
[Fact]
public void TryAdicionarEndereco_ComEnderecoValido_DeveRetornarTrue()
{
    var resultado = pessoa.TryAdicionarEndereco(endereco, out var erro);
    Assert.True(resultado);
    Assert.Null(erro); // [NotNullWhen(false)] garante que erro é null quando true
}
````
4. Proteção Contra Estados Inválidos
````
[Fact]
public void SetNome_ComValorNulo_DeveLancarExcecao()
{
    Assert.Throws<NullabilityContractException>(() => pessoa.Nome = null!);
}
````
## 🎯 Justificativa Técnica dos Atributos
[DisallowNull] em Propriedades Obrigatórias
Aplicado em: Pessoa.Nome, Endereco.Logradouro, Endereco.Cidade
Porque: Estas propriedades representam dados essenciais que nunca devem ser nulos. O atributo comunica essa intenção ao compilador e gera warnings se violado.

[NotNullWhen] em Try-Patterns
Aplicado em: TryAdicionarEndereco(endereco), TryAdicionarPessoa(pessoa)
Porque: Informa ao compilador que quando o método retorna true, o parâmetro não é nulo, permitindo uso seguro sem null-checks redundantes.

[MemberNotNull] em Métodos de Validação
Aplicado em: ValidarEstado(), ValidarEstadoSistema()
Porque: Garante ao compilador que campos marcados com [NotNull] estão inicializados após a execução do método, útil para inicialização tardia.

[NotNull] em Coleções
Aplicado em: Pessoa.Enderecos, SistemaValidacao.Pessoas
Porque: Comunica que estas coleções sempre retornam uma instância (podendo estar vazia), eliminando a necessidade de null-checks no código cliente.

## 🔍 Exemplo de Código com Contratos
````
public class Pessoa
{
    [DisallowNull]
    public string Nome { get; set; } = string.Empty;

    public bool TryAdicionarEndereco(
        [NotNullWhen(true)] Endereco? endereco, 
        [NotNullWhen(false)] out string? erro)
    {
        // O compilador sabe que 'endereco' não é nulo aqui
        // quando retorna true, e 'erro' não é nulo quando retorna false
    }
}
````
## 📝 Licença
Este projeto está sob a licença MIT. Veja o arquivo LICENSE para detalhes.

Desenvolvido como trabalho acadêmico de POO - Contratos de Nulidade em C# 🎓🔒
