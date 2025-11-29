namespace Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class NullabilityContractException : DomainException
{
    public NullabilityContractException(string message) : base(message) { }
}

public class InvalidEntityStateException : DomainException
{
    public InvalidEntityStateException(string message) : base(message) { }
}