namespace TestMobiBuy.Domain.Exceptions;

public class DomainErrorException : Exception
{
    public DomainErrorException(string message) : base(message)
    {
    }
}
