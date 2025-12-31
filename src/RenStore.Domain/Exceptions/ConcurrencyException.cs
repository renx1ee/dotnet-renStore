namespace RenStore.Domain.Exceptions;
 
public class ConcurrencyException(string message, Exception innerException) : Exception(message, innerException);