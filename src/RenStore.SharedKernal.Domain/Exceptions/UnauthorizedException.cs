namespace RenStore.SharedKernal.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() { }
    
    public UnauthorizedException(string param) : base(param) { }
}