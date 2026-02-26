namespace RenStore.SharedKernal.Domain.Exceptions;

public class ConcurrencyException : Exception
{
    public ConcurrencyException(string message) : base(message) { }
    
    public ConcurrencyException(string message, Exception ex) : base(message, ex) { }
}